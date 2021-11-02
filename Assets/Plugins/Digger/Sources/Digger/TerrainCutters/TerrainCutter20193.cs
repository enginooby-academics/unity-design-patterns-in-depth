using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Unity.Collections;
using UnityEngine;

namespace Digger.TerrainCutters
{
    [DefaultExecutionOrder(-11)] // Execute right before DiggerMasterRuntime to avoid issues when editing asynchronously
    public class TerrainCutter20193 : TerrainCutter
    {
        private const int LargeFileBufferSize = 32768;

        [SerializeField] private DiggerSystem digger;

        private TerrainData terrainData;
        private bool needsSync;
        private int holesResolution;
        private int sizeOfMesh;
        private int voxResolution;
        private int sizeOfMeshHoles;
        
        private Dictionary<Vector2i, bool[,]> holesPerChunk;
        private struct DelayedHoles
        {
            public int XBase;
            public int YBase;
            public bool[,] Holes;
        }
        private readonly Queue<DelayedHoles> delayedHolesToApply = new Queue<DelayedHoles>(250);

        public override void OnEnterPlayMode()
        {
            var transparencyMapPath = digger.GetTransparencyMapBackupPath("holes");
            SaveTo(transparencyMapPath);
        }

        public override void OnExitPlayMode()
        {
            var transparencyMapPath = digger.GetTransparencyMapBackupPath("holes");
            LoadFrom(transparencyMapPath);
        }

        public static TerrainCutter20193 CreateInstance(DiggerSystem digger)
        {
            var cutter = digger.gameObject.AddComponent<TerrainCutter20193>();
            cutter.digger = digger;
            cutter.Refresh();
            return cutter;
        }

        public override void Refresh()
        {
            terrainData = digger.Terrain.terrainData;
            holesResolution = terrainData.holesResolution;
            sizeOfMesh = digger.SizeOfMesh;
            voxResolution = digger.ResolutionMult;
            sizeOfMeshHoles = sizeOfMesh / voxResolution;
            var chunkRes = holesResolution / sizeOfMesh;
            holesPerChunk = new Dictionary<Vector2i, bool[,]>(chunkRes * chunkRes, new Vector2iComparer());
            // force initialisation of holes texture
            terrainData.SetHoles(0, 0, terrainData.GetHoles(0, 0, 1, 1));
        }

        private bool[,] GetCreateHoles(Vector2i chunkPosition, Vector3i voxelPosition)
        {
            if (holesPerChunk.TryGetValue(chunkPosition, out var holes)) {
                return holes;
            }

            holes = terrainData.GetHoles(voxelPosition.x / voxResolution, voxelPosition.z / voxResolution, sizeOfMeshHoles, sizeOfMeshHoles);
            holesPerChunk.Add(chunkPosition, holes);
            return holes;
        }
        
        public void Cut(NativeArray<int> chunkHoles, Vector3i voxelPosition, Vector3i chunkPosition)
        {
            var holes = GetCreateHoles(new Vector2i(chunkPosition.x, chunkPosition.z), voxelPosition);
            if (voxResolution == 1) { // this is just a quick-win
                for (var x = 0; x < sizeOfMeshHoles; ++x) {
                    for (var z = 0; z < sizeOfMeshHoles; ++z) {
                        holes[x, z] = holes[x, z] &&
                                      chunkHoles[(x*voxResolution) * sizeOfMesh + (z*voxResolution)] == 0;
                    }
                }
            } else {
                for (var x = 0; x < sizeOfMeshHoles; ++x) {
                    for (var z = 0; z < sizeOfMeshHoles; ++z) {
                        for (var rx = 0; rx < voxResolution; ++rx) {
                            for (var rz = 0; rz < voxResolution; ++rz) {
                                holes[x, z] = holes[x, z] &&
                                              chunkHoles[(x*voxResolution + rx) * sizeOfMesh + (z*voxResolution + rz)] == 0;
                            }
                        }
                    }
                }
            }

            delayedHolesToApply.Enqueue(new DelayedHoles
            {
                XBase = voxelPosition.x / voxResolution,
                YBase = voxelPosition.z / voxResolution,
                Holes = holes
            });
            Utils.Profiler.BeginSample("[Dig] Cutter20193.Cut");
            needsSync = true;
            Utils.Profiler.EndSample();
        }
        
        public void UnCut(NativeArray<int> chunkHoles, Vector3i voxelPosition, Vector3i chunkPosition)
        {
            var holes = GetCreateHoles(new Vector2i(chunkPosition.x, chunkPosition.z), voxelPosition);
            for (var x = 0; x < sizeOfMeshHoles; ++x) {
                for (var z = 0; z < sizeOfMeshHoles; ++z) {
                    for (var rx = 0; rx < voxResolution; ++rx) {
                        for (var rz = 0; rz < voxResolution; ++rz) {
                            holes[x, z] = holes[x, z] || 
                                          chunkHoles[(x*voxResolution + rx) * sizeOfMesh + (z*voxResolution + rz)] != 0;
                        }
                    }
                }
            }

            Utils.Profiler.BeginSample("[Dig] Cutter20193.UnCut");
            terrainData.SetHolesDelayLOD(voxelPosition.x / voxResolution, voxelPosition.z / voxResolution, holes);
            needsSync = true;
            Utils.Profiler.EndSample();
        }

        protected override void ApplyInternal(bool persist)
        {
            Utils.Profiler.BeginSample("[Dig] Cutter20193.Apply");
            if (needsSync) {
                needsSync = false;
                while (delayedHolesToApply.Count > 0) {
                    var toApply = delayedHolesToApply.Dequeue();
                    terrainData.SetHolesDelayLOD(toApply.XBase, toApply.YBase, toApply.Holes);
                }
                terrainData.SyncTexture(TerrainData.HolesTextureName);
            }

            Utils.Profiler.EndSample();
        }

        public override void LoadFrom(string path)
        {
            if (!File.Exists(path))
                return;

            Refresh();
            var resolution = digger.Terrain.terrainData.holesResolution;
            var holes = new bool[resolution,resolution];
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, LargeFileBufferSize)) {
                using (var reader = new BinaryReader(stream, Encoding.Default)) {
                    for (var x = 0; x < resolution; ++x) {
                        for (var z = 0; z < resolution; ++z) {
                            holes[z, x] = reader.ReadBoolean();
                        }
                    }
                }
            }

            digger.Terrain.terrainData.SetHoles(0, 0, holes);
        }

        public override void SaveTo(string path)
        {
            var resolution = digger.Terrain.terrainData.holesResolution;
            var holes = digger.Terrain.terrainData.GetHoles(0, 0, resolution, resolution);
            if (holes == null)
                return;

            if (File.Exists(path))
                File.Delete(path);

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, LargeFileBufferSize)) {
                using (var writer = new BinaryWriter(stream, Encoding.Default)) {
                    for (var x = 0; x < resolution; ++x) {
                        for (var z = 0; z < resolution; ++z) {
                            writer.Write(holes[z, x]);
                        }
                    }
                }
            }
        }

        public override void Clear()
        {
#if UNITY_EDITOR
            Utils.Profiler.BeginSample("[Dig] Cutter.Clear");
            Refresh();
            var resolution = digger.Terrain.terrainData.holesResolution;
            var holes = digger.Terrain.terrainData.GetHoles(0, 0, resolution, resolution);
            for (var x = 0; x < resolution; ++x) {
                for (var z = 0; z < resolution; ++z) {
                    holes[z, x] = true;
                }
            }

            digger.Terrain.terrainData.SetHoles(0, 0, holes);
            Utils.Profiler.EndSample();
#endif
        }
    }
}