using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Digger.HeightFeeders;
using Digger.TerrainCutters;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Digger
{
    public class DiggerSystem : MonoBehaviour
    {
        public const int DiggerVersion = 10031; // 10000 and above means "DiggerPRO"
        public const string VoxelFileExtension = "vox3";
        public const string VoxelFileExtensionLegacyV1 = "vox";
        public const string VoxelFileExtensionLegacyV2 = "vox2";
        private const string VoxelMetadataFileExtension = "vom";
        private const string VersionFileExtension = "ver";
        private const int UndoStackSize = 15;

        private Dictionary<Vector3i, Chunk> chunks;
        private IHeightFeeder heightFeeder;
        private HashSet<VoxelChunk> chunksToPersist;
        private Dictionary<Collider, int> colliderStates;

        private readonly Dictionary<Vector3i, Chunk> builtChunks =
            new Dictionary<Vector3i, Chunk>(new Vector3iComparer());

        private bool disablePersistence;
        private Bounds bounds = new Bounds();
        private bool needRecordUndo;
        private Stopwatch stopwatch = new Stopwatch();

        [SerializeField] private DiggerMaster master;
        [SerializeField] private string guid;
        [SerializeField] private long version = 1;
        [SerializeField] private string basePathData;

        [SerializeField] private TerrainCutter cutter;

        [SerializeField] private Vector3 heightmapScale;
        [SerializeField] private Vector3 holeMapScale;

        [SerializeField] public Terrain Terrain;
        [SerializeField] public Material[] Materials;
        [SerializeField] private TerrainMaterialType materialType;
        [SerializeField] private Texture2D[] terrainTextures;
        [SerializeField] private Vector3i[] chunksInStreamingAssets;
        [SerializeField] private string persistenceSubPath;
        [SerializeField] public bool ShowDebug;


        private bool kernelActionNeedsApply;

        public string Guid {
            get => guid;
            set => guid = value;
        }

        public Vector3 HeightmapScale => heightmapScale;
        public Vector3 HoleMapScale => holeMapScale;

        public Vector3 CutMargin => new Vector3(Math.Max(2f, 2.1f * holeMapScale.x),
                                                Math.Max(2f, 2.1f * holeMapScale.y),
                                                Math.Max(2f, 2.1f * holeMapScale.z));

        public TerrainCutter Cutter => cutter;
        public IHeightFeeder HeightFeeder => heightFeeder;

        public Texture2D[] TerrainTextures {
            set => terrainTextures = value;
            get => terrainTextures;
        }

        public float ScreenRelativeTransitionHeightLod0 => master.ScreenRelativeTransitionHeightLod0;
        public float ScreenRelativeTransitionHeightLod1 => master.ScreenRelativeTransitionHeightLod1;
        public int ColliderLodIndex => master.ColliderLodIndex;
        public bool CreateLODs => master.CreateLODs;
        private int Layer => master.Layer;
        private string Tag => master.ChunksTag;
        public bool EnableOcclusionCulling => master.EnableOcclusionCulling;

        public int SizeOfMesh => master.SizeOfMesh;
        public int SizeVox => master.SizeVox;
        public int ResolutionMult => master.ResolutionMult;

        public int DefaultNavMeshArea { get; set; }

        public TerrainMaterialType MaterialType {
            get => materialType;
            set => materialType = value;
        }

        private string BaseFolder => $"{guid}";

        public string BasePathData {
            get {
                // Handle legacy
                if (string.IsNullOrEmpty(basePathData)) {
                    basePathData = ComputeBasePathData();
                }

                return basePathData;
            }
        }

        private string ComputeBasePathData()
        {
            if (!master) master = FindObjectOfType<DiggerMaster>();
            return Path.Combine(master.SceneDataPath, BaseFolder);
        }

        private string InternalPathData => Path.Combine(BasePathData, ".internal");

        private string StreamingAssetsPathData =>
            Path.Combine(Application.streamingAssetsPath, "DiggerData", BaseFolder);

        private string PersistentRuntimePathData {
            get {
                if (persistenceSubPath != "") {
                    return Path.Combine(Application.persistentDataPath, "DiggerData", persistenceSubPath, BaseFolder);
                }

                return Path.Combine(Application.persistentDataPath, "DiggerData", BaseFolder);
            }
        }

        public long Version => version;
        public long PreviousVersion => version - 1;

        private int TerrainChunkWidth =>
            Terrain.terrainData.heightmapResolution * master.ResolutionMult / SizeOfMesh - 1;

        private int TerrainChunkHeight =>
            Terrain.terrainData.heightmapResolution * master.ResolutionMult / SizeOfMesh - 1;

        public bool IsInitialized => Terrain != null && master != null && chunks != null && cutter != null &&
                                     heightFeeder != null &&
                                     chunksToPersist != null;

        public Bounds Bounds => bounds;

        public string PersistenceSubPath {
            get => persistenceSubPath;
            set => persistenceSubPath = value;
        }

        private string GetPathDiggerVersionFile()
        {
            return Path.Combine(BasePathData, "digger_version.asset");
        }

        private string GetPathCurrentVersionFile()
        {
            return Path.Combine(BasePathData, "current_version.asset");
        }

        private string GetPathVersionFile(long v)
        {
            return Path.Combine(InternalPathData, $"version_{v}.{VersionFileExtension}");
        }

        public string GetEditorOnlyPathVoxelFile(Vector3i chunkPosition)
        {
            return Path.Combine(InternalPathData, $"{Chunk.GetName(chunkPosition)}.{VoxelFileExtension}");
        }

        public string GetPathVoxelFile(Vector3i chunkPosition, bool forPersistence)
        {
#if UNITY_EDITOR
            return Path.Combine(InternalPathData, $"{Chunk.GetName(chunkPosition)}.{VoxelFileExtension}");
#else
            if (forPersistence) {
                return Path.Combine(PersistentRuntimePathData, $"{Chunk.GetName(chunkPosition)}.{VoxelFileExtension}");
            }

            var path = Path.Combine(PersistentRuntimePathData, $"{Chunk.GetName(chunkPosition)}.{VoxelFileExtension}");
            if (!File.Exists(path)) {
                path = Path.Combine(StreamingAssetsPathData, $"{Chunk.GetName(chunkPosition)}.{VoxelFileExtension}");
            }
            return path;
#endif
        }

        public string GetPathVoxelMetadataFile(Vector3i chunkPosition, bool forPersistence)
        {
            return Path.ChangeExtension(GetPathVoxelFile(chunkPosition, forPersistence), VoxelMetadataFileExtension);
        }

        public string GetEditorOnlyPathVoxelMetadataFile(Vector3i chunkPosition)
        {
            return Path.ChangeExtension(GetEditorOnlyPathVoxelFile(chunkPosition), VoxelMetadataFileExtension);
        }

        public string GetPathVersionedVoxelFile(Vector3i chunkPosition, long v)
        {
            return Path.ChangeExtension(GetEditorOnlyPathVoxelFile(chunkPosition), $"{VoxelFileExtension}_v{v}");
        }

        public string GetPathVersionedVoxelMetadataFile(Vector3i chunkPosition, long v)
        {
            return Path.ChangeExtension(GetEditorOnlyPathVoxelMetadataFile(chunkPosition),
                                        $"{VoxelMetadataFileExtension}_v{v}");
        }

        public string GetTransparencyMapPath()
        {
            return Path.Combine(BasePathData, "TransparencyMap.asset");
        }

        public string GetTransparencyMapBackupPath(string ext = "asset")
        {
            return Path.Combine(BasePathData, $"TransparencyMapBackup.{ext}");
        }

        public string GetVersionedTransparencyMapPath(long v)
        {
            return Path.Combine(InternalPathData, $"TransparencyMap.asset_{v}");
        }

        public string GetTransparencyMapRuntimePath()
        {
            return Path.Combine(PersistentRuntimePathData, "TransparencyMap.asset");
        }

        public Bounds GetChunkBounds()
        {
            var worldSize = Vector3.one * SizeOfMesh;
            worldSize.x *= HeightmapScale.x;
            worldSize.z *= HeightmapScale.z;
            return new Bounds(worldSize * 0.5f, worldSize);
        }

        private void Awake()
        {
            colliderStates = new Dictionary<Collider, int>(new ColliderComparer());
        }

        public void PersistAndRecordUndo(bool force, bool removeUselessChunks)
        {
#if UNITY_EDITOR
            if (!needRecordUndo && !force)
                return;

            Utils.Profiler.BeginSample("PersistAndRecordUndo");
            CreateDirs();

            if (removeUselessChunks) {
                RemoveUselessChunks();
            }

            foreach (var chunkToPersist in chunksToPersist) {
                if (chunks.ContainsKey(chunkToPersist.ChunkPosition))
                    chunkToPersist.Persist();
            }

            chunksToPersist.Clear();

            DeleteOtherVersions(true, version - UndoStackSize);

            var versionInfo = new VersionInfo
            {
                Version = version,
                AliveChunks = chunks.Keys.ToList()
            };

            File.WriteAllText(GetPathVersionFile(version), JsonUtility.ToJson(versionInfo));

            Utils.Profiler.BeginSample("Save cutter");
            cutter.SaveTo(GetVersionedTransparencyMapPath(version));
            Utils.Profiler.EndSample();

            Utils.Profiler.BeginSample("RegisterCompleteObjectUndo");
            Undo.RegisterCompleteObjectUndo(this, "Digger edit");
            Utils.Profiler.EndSample();

            Utils.Profiler.BeginSample("PersistVersion");
            ++version;
            PersistVersion();
            Utils.Profiler.EndSample();

            needRecordUndo = false;
            Utils.Profiler.EndSample();
#endif
        }

        #region DiggerPRO

        public void PersistAtRuntime()
        {
            if (disablePersistence)
                return;

            if (!Directory.Exists(PersistentRuntimePathData)) {
                Directory.CreateDirectory(PersistentRuntimePathData);
            }

            foreach (var chunkToPersist in chunksToPersist) {
                chunkToPersist.Persist();
            }

            chunksToPersist.Clear();

            cutter.SaveTo(GetTransparencyMapRuntimePath());
        }

        public void DeleteDataPersistedAtRuntime()
        {
            if (Directory.Exists(PersistentRuntimePathData)) {
                Directory.Delete(PersistentRuntimePathData, true);
            }
        }

        #endregion

        public void DoUndo()
        {
#if UNITY_EDITOR
            if (!CanUndo) {
                Utils.D.Log($"Cannot undo terrain {Terrain.name} to previous version {PreviousVersion}");
                ++version;
                PersistVersion();
                Undo.ClearUndo(this);
                return;
            }

            var versionBeforeUndo = GetLastPersistedVersion();
            if (versionBeforeUndo == version) {
                Utils.D.Log($"No need to sync terrain {Terrain.name} to version {PreviousVersion}");
                return;
            }

            Utils.D.Log($"Sync terrain {Terrain.name} to version {PreviousVersion}");
            PersistVersion();
            var versionInfo = JsonUtility.FromJson<VersionInfo>(File.ReadAllText(GetPathVersionFile(PreviousVersion)));

            UndoRedoFiles();
            Reload(LoadType.Minimal_and_LoadVoxels_and_RebuildMeshes);
            SyncChunksWithVersion(versionInfo);
#endif
        }

        private bool CanUndo => Application.isEditor && Terrain && cutter && Directory.Exists(InternalPathData) &&
                                File.Exists(GetPathVersionFile(PreviousVersion));

        public void PersistDiggerVersion()
        {
#if UNITY_EDITOR
            CreateDirs();
            EditorUtils.CreateOrReplaceAsset(new TextAsset(DiggerVersion.ToString()), GetPathDiggerVersionFile());
#endif
        }

        public int GetDiggerVersion()
        {
#if UNITY_EDITOR
            var verAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(GetPathDiggerVersionFile());
            if (verAsset) {
                return Convert.ToInt32(verAsset.text);
            }
#endif
            return 0;
        }

        private void PersistVersion()
        {
#if UNITY_EDITOR
            CreateDirs();
            EditorUtils.CreateOrReplaceAsset(new TextAsset(version.ToString()), GetPathCurrentVersionFile());
            Utils.D.Log($"PersistVersion of {Terrain.name} with value {version}");
#endif
        }

        private long GetLastPersistedVersion()
        {
#if UNITY_EDITOR
            var verAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(GetPathCurrentVersionFile());
            if (verAsset) {
                return Convert.ToInt64(verAsset.text);
            }
#endif
            return 0;
        }

        private void UndoRedoFiles()
        {
            var dir = new DirectoryInfo(InternalPathData);
            foreach (var file in dir.EnumerateFiles($"*.{VoxelFileExtension}_v{PreviousVersion}")) {
                var bytesFilePath = Path.ChangeExtension(file.FullName, VoxelFileExtension);
                File.Copy(file.FullName, bytesFilePath, true);
            }

            foreach (var file in dir.EnumerateFiles($"*.{VoxelMetadataFileExtension}_v{PreviousVersion}")) {
                var bytesFilePath = Path.ChangeExtension(file.FullName, VoxelMetadataFileExtension);
                File.Copy(file.FullName, bytesFilePath, true);
            }

            cutter.LoadFrom(GetVersionedTransparencyMapPath(PreviousVersion));
        }

        private void SyncChunksWithVersion(VersionInfo versionInfo)
        {
            // Remove chunks that don't exist in this version
            var chunksToRemove = new List<Chunk>();
            foreach (var chunk in chunks) {
                if (!versionInfo.AliveChunks.Contains(chunk.Key)) {
                    chunksToRemove.Add(chunk.Value);
                }
            }

            foreach (var chunk in chunksToRemove) {
                RemoveChunk(chunk);
            }

            ComputeBounds();
        }


        private void DeleteOtherVersions(bool lower, long comparandVersion)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                return;

            if (!Directory.Exists(InternalPathData))
                return;

            Utils.Profiler.BeginSample("[Dig] DeleteOtherVersions");

            var dir = new DirectoryInfo(InternalPathData);
            foreach (var verFile in dir.EnumerateFiles($"version_*.{VersionFileExtension}")) {
                long versionToRemove;
                if (long.TryParse(verFile.Name.Replace("version_", "").Replace($".{VersionFileExtension}", ""),
                                  out versionToRemove)
                    && (!lower && versionToRemove >= comparandVersion ||
                        lower && versionToRemove <= comparandVersion)) {
                    foreach (var voxFile in dir.EnumerateFiles($"*.{VoxelFileExtension}_v{versionToRemove}")) {
                        voxFile.Delete();
                    }

                    foreach (var voxMetadataFile in dir.EnumerateFiles(
                        $"*.{VoxelMetadataFileExtension}_v{versionToRemove}")) {
                        voxMetadataFile.Delete();
                    }

                    if (File.Exists(GetVersionedTransparencyMapPath(versionToRemove))) {
                        File.Delete(GetVersionedTransparencyMapPath(versionToRemove));
                    }

                    verFile.Delete();
                }
            }

            Utils.Profiler.EndSample();
#endif
        }

        /// <summary>
        /// PreInit setup mandatory fields Terrain, Master and Guid, and it create directories.
        /// This is idempotent and can be called several times.
        /// </summary>
        public void PreInit(bool enablePersistence)
        {
            this.disablePersistence = !enablePersistence;
            Terrain = transform.parent.GetComponent<Terrain>();
            if (!Terrain) {
                Debug.LogError("DiggerSystem component can only be added as a child of a terrain.");
                return;
            }

            master = FindObjectOfType<DiggerMaster>();
            if (!master) {
                Debug.LogError("A DiggerMaster is required in the scene.");
                return;
            }

#if UNITY_EDITOR
            if (string.IsNullOrEmpty(guid)) {
                guid = GUID.Generate().ToString();
            }
#endif

            CreateDirs();
        }

        /// <summary>
        /// Initialize Digger and eventually reloads chunks
        /// </summary>
        public void Init(LoadType loadType)
        {
            var terrainData = Terrain.terrainData;
            heightmapScale = terrainData.heightmapScale / master.ResolutionMult;
            heightmapScale.y = 1f / master.ResolutionMult;
            holeMapScale =
                new Vector3(terrainData.size.x / terrainData.holesResolution, 1f, terrainData.size.z / terrainData.holesResolution);

            var persistedVersion = GetLastPersistedVersion();
            if (persistedVersion != Version && CanUndo) {
                Debug.LogWarning(
                    $"Digger of terrain {terrainData.name} Version is {Version} but PersistedVersion is {persistedVersion}. Re-syncing...");
                DoUndo();
            } else {
                Reload(loadType);
            }
        }


        private void Reload(LoadType loadType)
        {
            Utils.D.Log($"Reloading with LoadType = {loadType}");
            Utils.Profiler.BeginSample("[Dig] Reload");
            CreateDirs();

            // check terrain scale and Digger transform
            gameObject.layer = Layer;
            Terrain.transform.rotation = Quaternion.identity;
            Terrain.transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            if (cutter && !TerrainCutter.IsGoodVersion(cutter)) {
                DestroyImmediate(cutter);
                cutter = null;
            }

            if (!cutter) {
                cutter = GetComponent<TerrainCutter>();
                if (!cutter) {
                    cutter = TerrainCutter.Create(Terrain, this);
                }
            }

            cutter.Refresh();
#if !UNITY_EDITOR
            if (File.Exists(GetTransparencyMapRuntimePath())) {
                cutter.LoadFrom(GetTransparencyMapRuntimePath());
            }
#endif
            cutter.Apply(true);
            chunks = new Dictionary<Vector3i, Chunk>(100, new Vector3iComparer());
            heightFeeder = new TerrainHeightFeeder(Terrain.terrainData, master.ResolutionMult);
            chunksToPersist = new HashSet<VoxelChunk>();
            var children = transform.Cast<Transform>().ToList();
            foreach (var child in children) {
                var chunk = child.GetComponent<Chunk>();
                if (chunk) {
                    if (chunk.Digger != this) {
                        Debug.LogError("Chunk is badly defined. Missing/wrong cutter and/or digger reference.");
                    }

                    if (!loadType.RebuildMeshes) {
                        chunks.Add(chunk.ChunkPosition, chunk);
                    } else {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }

            LoadChunks(loadType);

            ComputeBounds();
            UpdateStaticEditorFlags();

            Utils.Profiler.EndSample();
        }

        public float[] GrabAlphamaps(int2 from, int2 to, out int alphamapCount)
        {
            var size = to - from;
            var tData = Terrain.terrainData;
            var tAlphamaps = tData.GetAlphamaps(from.x, from.y, size.x, size.y);

            var sx = tAlphamaps.GetLength(1);
            var sz = tAlphamaps.GetLength(0);
            alphamapCount = tAlphamaps.GetLength(2);
            var alphamaps = new float[size.x * size.y * alphamapCount];
            for (var x = 0; x < size.x; ++x) {
                for (var z = 0; z < size.y; ++z) {
                    for (var map = 0; map < alphamapCount; ++map) {
                        var a = x < sx && z < sz ? tAlphamaps[z, x, map] : 0f;
                        alphamaps[x * size.y * alphamapCount + z * alphamapCount + map] = a;
                    }
                }
            }

            return alphamaps;
        }

        internal void AddNavMeshSources(List<NavMeshBuildSource> sources)
        {
            foreach (var chunk in chunks) {
                var navSrc = chunk.Value.NavMeshBuildSource;
                if (navSrc.sourceObject) {
                    sources.Add(navSrc);
                }
            }
        }

        private bool GetOrCreateChunk(Vector3i position, out Chunk chunk)
        {
            Utils.Profiler.BeginSample("[Dig] GetOrCreateChunk");
            if (!chunks.TryGetValue(position, out chunk)) {
                chunk = Chunk.CreateChunk(position, this, Terrain, Materials, Layer, Tag);
                chunks.Add(position, chunk);
                var b = GetChunkBounds();
                ExpandBounds(chunk.WorldPosition, chunk.WorldPosition + b.size);
                Utils.Profiler.EndSample();
                return false;
            }

            Utils.Profiler.EndSample();
            return true;
        }

        internal bool GetChunk(Vector3i position, out Chunk chunk)
        {
            return chunks.TryGetValue(position, out chunk);
        }

        public bool Modify(BrushType brush, ActionType action, float intensity, Vector3 operationWorldPosition,
                           float radius, float coneHeight, bool upsideDown, int textureIndex, bool cutDetails, bool isTargetIntensity)
        {
            var area = GetAreaToModify(action, intensity, operationWorldPosition, radius);
            if (!area.NeedsModification)
                return false;

            var enumerator = Modify(brush, action, intensity, operationWorldPosition, radius, coneHeight, upsideDown, textureIndex, cutDetails, isTargetIntensity, false, area);
            enumerator.MoveNext();
            ApplyModify();
            return true;
        }

        internal IEnumerator ModifyAsync(BrushType brush, ActionType action, float intensity, Vector3 operationWorldPosition,
                                         float radius, float coneHeight, bool upsideDown, int textureIndex, bool cutDetails, bool isTargetIntensity, ModificationArea area)
        {
            return Modify(brush, action, intensity, operationWorldPosition, radius, coneHeight, upsideDown, textureIndex, cutDetails, isTargetIntensity, true, area);
        }

        internal ModificationArea GetAreaToModify(ActionType action, float intensity, Vector3 operationWorldPosition, float radius)
        {
            if (action != ActionType.Paint && action != ActionType.PaintHoles && intensity < 0f) {
                Debug.LogWarning("Opacity can only be negative when action type is 'Paint' or 'PaintHoles'");
                return new ModificationArea
                {
                    NeedsModification = false
                };
            }

            var operationTerrainPosition = operationWorldPosition - Terrain.transform.position;
            var p = operationTerrainPosition;
            p.x /= heightmapScale.x;
            p.z /= heightmapScale.z;
            var voxelRadius = (int) ((radius + Math.Max(CutMargin.x, CutMargin.z)) /
                                     Math.Min(heightmapScale.x, heightmapScale.z)) + 1;
            var voxMargin = new Vector3i(voxelRadius, (int) (radius + CutMargin.y) + 1, voxelRadius);
            var voxMin = new Vector3i(p) - voxMargin;
            var voxMax = new Vector3i(p) + voxMargin;

            var min = voxMin / SizeOfMesh;
            var max = voxMax / SizeOfMesh;
            if (voxMin.x < 0)
                min.x--;
            if (voxMin.y < 0)
                min.y--;
            if (voxMin.z < 0)
                min.z--;
            if (voxMax.x < 0)
                max.x--;
            if (voxMax.y < 0)
                max.y--;
            if (voxMax.z < 0)
                max.z--;

            if (max.x < 0 || max.z < 0 || min.x > TerrainChunkWidth || min.z > TerrainChunkHeight) {
                return new ModificationArea
                {
                    NeedsModification = false
                };
            }

            var minMaxHeight = GetMinMaxHeightWithin(voxMin, voxMax);
            if (min.y <= ((int) minMaxHeight.y + 1) / SizeOfMesh && min.y > ((int) minMaxHeight.x - 1) / SizeOfMesh) {
                min.y = ((int) minMaxHeight.x - 1) / SizeOfMesh;
            }

            if (max.y >= ((int) minMaxHeight.x - 1) / SizeOfMesh && max.y < ((int) minMaxHeight.y + 1) / SizeOfMesh) {
                max.y = ((int) minMaxHeight.y + 1) / SizeOfMesh;
            }

            return new ModificationArea
            {
                NeedsModification = true,
                Min = min,
                Max = max,
                OperationTerrainPosition = operationTerrainPosition
            };
        }

        private IEnumerator Modify(BrushType brush, ActionType action, float intensity, Vector3 operationWorldPosition,
                                   float radius, float coneHeight, bool upsideDown, int textureIndex, bool cutDetails, bool isTargetIntensity, bool runAsync,
                                   ModificationArea area)
        {
            needRecordUndo = true;
            CreateDirs();
            DeleteOtherVersions(false, version);

            var min = area.Min;
            var max = area.Max;

            if (min.x < 0)
                min.x = 0;
            if (min.z < 0)
                min.z = 0;
            if (max.x > TerrainChunkWidth)
                max.x = TerrainChunkWidth;
            if (max.z > TerrainChunkHeight)
                max.z = TerrainChunkHeight;

            stopwatch.Restart();
            for (var x = min.x; x <= max.x; ++x) {
                for (var z = min.z; z <= max.z; ++z) {
                    for (var y = min.y; y <= max.y; ++y) {
                        var cp = new Vector3i(x, y, z);
                        if (builtChunks.ContainsKey(cp))
                            continue;

                        GetOrCreateChunk(cp, out var chunk);
                        builtChunks.Add(cp, chunk);
                        chunk.Modify(brush, action, intensity, area.OperationTerrainPosition, radius, coneHeight, upsideDown,
                                     textureIndex, cutDetails, isTargetIntensity);

                        if (runAsync && stopwatch.ElapsedMilliseconds > 4) {
                            stopwatch.Stop();
                            yield return null; // Continue on next frame
                            stopwatch.Restart();
                        }
                    }
                }
            }

            switch (action) {
                case ActionType.Smooth:
                case ActionType.BETA_Sharpen:
                    kernelActionNeedsApply = true;
                    break;
            }

            stopwatch.Stop();
            if (runAsync && stopwatch.ElapsedMilliseconds > 1) {
                yield return null; // Skip one frame
            }
        }

        internal void ApplyModify()
        {
            foreach (var builtChunk in builtChunks) {
                builtChunk.Value.ApplyModify();
                if (kernelActionNeedsApply) {
                    builtChunk.Value.PrepareKernelOperation();
                }
            }

            builtChunks.Clear();
            cutter.Apply(true);
        }

        public bool IsChunkBelongingToMe(Vector3i chunkPosition)
        {
            return chunkPosition.x >= 0 && chunkPosition.x <= TerrainChunkWidth &&
                   chunkPosition.z >= 0 && chunkPosition.z <= TerrainChunkHeight;
        }

        public DiggerSystem GetNeighborAt(Vector3i chunkPosition)
        {
            if (chunkPosition.x < 0) {
                if (chunkPosition.z < 0) {
                    if (Terrain.leftNeighbor)
                        return GetDiggerSystemOf(Terrain.leftNeighbor.bottomNeighbor);
                    if (Terrain.bottomNeighbor)
                        return GetDiggerSystemOf(Terrain.bottomNeighbor.leftNeighbor);
                } else if (chunkPosition.z > TerrainChunkHeight) {
                    if (Terrain.leftNeighbor)
                        return GetDiggerSystemOf(Terrain.leftNeighbor.topNeighbor);
                    if (Terrain.topNeighbor)
                        return GetDiggerSystemOf(Terrain.topNeighbor.leftNeighbor);
                } else {
                    return GetDiggerSystemOf(Terrain.leftNeighbor);
                }
            } else if (chunkPosition.x > TerrainChunkWidth) {
                if (chunkPosition.z < 0) {
                    if (Terrain.rightNeighbor)
                        return GetDiggerSystemOf(Terrain.rightNeighbor.bottomNeighbor);
                    if (Terrain.bottomNeighbor)
                        return GetDiggerSystemOf(Terrain.bottomNeighbor.rightNeighbor);
                } else if (chunkPosition.z > TerrainChunkHeight) {
                    if (Terrain.rightNeighbor)
                        return GetDiggerSystemOf(Terrain.rightNeighbor.topNeighbor);
                    if (Terrain.topNeighbor)
                        return GetDiggerSystemOf(Terrain.topNeighbor.rightNeighbor);
                } else {
                    return GetDiggerSystemOf(Terrain.rightNeighbor);
                }
            } else {
                if (chunkPosition.z < 0) {
                    return GetDiggerSystemOf(Terrain.bottomNeighbor);
                } else if (chunkPosition.z > TerrainChunkHeight) {
                    return GetDiggerSystemOf(Terrain.topNeighbor);
                } else {
                    return this;
                }
            }

            return null;
        }

        public Vector3i ToChunkPosition(Vector3 worldPosition)
        {
            var p = worldPosition - Terrain.transform.position;
            p.x /= heightmapScale.x;
            p.z /= heightmapScale.z;
            return new Vector3i(p) / SizeOfMesh;
        }

        public Vector3 ToWorldPosition(Vector3i chunkPosition)
        {
            Vector3 p = chunkPosition * SizeOfMesh;
            p.x *= heightmapScale.x;
            p.z *= heightmapScale.z;
            return p + Terrain.transform.position;
        }

        private static DiggerSystem GetDiggerSystemOf(Terrain terrain)
        {
            return !terrain ? null : terrain.GetComponentInChildren<DiggerSystem>();
        }

        public void RemoveTreesInSphere(Vector3 center, float radius)
        {
            var tData = Terrain.terrainData;
            for (var i = 0; i < tData.treeInstanceCount; ++i) {
                var treeInstance = tData.GetTreeInstance(i);
                var position = TerrainUtils.UVToWorldPosition(tData, treeInstance.position);
                if (Vector3.Distance(position, center) < radius) {
                    treeInstance.heightScale = 0f;
                    treeInstance.widthScale = 0f;
                    tData.SetTreeInstance(i, treeInstance);
                }
            }
        }

        private float2 GetMinMaxHeightWithin(Vector3i minVox, Vector3i maxVox)
        {
            var minMax = new float2(float.MaxValue, float.MinValue);
            for (var x = minVox.x; x <= maxVox.x; ++x) {
                for (var z = minVox.z; z <= maxVox.z; ++z) {
                    var h = heightFeeder.GetHeight(x, z);
                    minMax.x = math.min(minMax.x, h);
                    minMax.y = math.max(minMax.y, h);
                }
            }

            return minMax;
        }

        private void ComputeBounds()
        {
            var firstIteration = true;
            var b = GetChunkBounds();
            foreach (var chunk in chunks) {
                var min = chunk.Value.WorldPosition;
                var max = min + b.size;
                if (firstIteration) {
                    firstIteration = false;
                    bounds.SetMinMax(min, max);
                    continue;
                }

                ExpandBounds(min, max);
            }
        }

        private void ExpandBounds(Vector3 min, Vector3 max)
        {
            if (bounds.min.x < min.x) {
                min.x = bounds.min.x;
            }

            if (bounds.min.y < min.y) {
                min.y = bounds.min.y;
            }

            if (bounds.min.z < min.z) {
                min.z = bounds.min.z;
            }

            if (bounds.max.x > max.x) {
                max.x = bounds.max.x;
            }

            if (bounds.max.y > max.y) {
                max.y = bounds.max.y;
            }

            if (bounds.max.z > max.z) {
                max.z = bounds.max.z;
            }

            bounds.SetMinMax(min, max);
        }

        public void EnsureChunkExists(Vector3i chunkPosition)
        {
            if (chunkPosition.x < 0 || chunkPosition.z < 0 || chunkPosition.x >= TerrainChunkWidth ||
                chunkPosition.z >= TerrainChunkHeight)
                return;

            Chunk chunk;
            if (!GetOrCreateChunk(chunkPosition, out chunk)) {
                chunk.CreateWithoutOperation();
            }
        }

        public void EnsureChunkWillBePersisted(VoxelChunk voxelChunk)
        {
            Utils.Profiler.BeginSample("[Dig] EnsureChunkWillBePersisted");
            if (!disablePersistence) {
                chunksToPersist.Add(voxelChunk);
            }

            Utils.Profiler.EndSample();
        }

        private void RemoveUselessChunks()
        {
            var chunksToRemove = new List<Chunk>();
            foreach (var chunk in chunks) {
                if (IsUseless(chunk.Key)) {
                    Utils.D.Log("[Digger] Cleaning chunk at " + chunk.Key);
                    chunksToRemove.Add(chunk.Value);
                }
            }

            foreach (var chunk in chunksToRemove) {
                RemoveChunk(chunk);
            }

            ComputeBounds();
        }

        private void RemoveChunk(Chunk chunk)
        {
            chunks.Remove(chunk.ChunkPosition);
            var file = GetPathVoxelFile(chunk.ChunkPosition, true);
            if (File.Exists(file)) {
                File.Delete(file);
            }

            file = GetPathVoxelMetadataFile(chunk.ChunkPosition, true);
            if (File.Exists(file)) {
                File.Delete(file);
            }

            if (Application.isEditor) {
                DestroyImmediate(chunk.gameObject);
            } else {
                Destroy(chunk.gameObject);
            }
        }

        private bool IsUseless(Vector3i chunkPosition)
        {
            if (!chunks.TryGetValue(chunkPosition, out var chunk))
                return false; // if it doesn't exist, it doesn't need to be removed
            if (chunk.HasVisualMesh)
                return false; // if it has a visual mesh, it must not be removed
            foreach (var direction in Vector3i.allDirections) {
                if (!chunks.TryGetValue(chunkPosition + direction, out var neighbour))
                    continue;
                if (neighbour.NeedsNeighbour(-direction))
                    return false; // if one of the chunk's neighbours need it, it must not be removed
            }

            // we do this test last because it is slow
            if (chunk.VoxelChunk && chunk.VoxelChunk.HasAlteredVoxels())
                return false; // if it has altered voxels, it must not be removed

            return true;
        }

        private void LoadChunks(LoadType loadType)
        {
#if UNITY_EDITOR
            var path = InternalPathData;
            if (!Directory.Exists(path))
                return;

            if (chunks == null) {
                Debug.LogError("Chunks dico should not be null in loading");
                return;
            }


            if (loadType.LoadVoxels) {
                foreach (var chunk in chunks) {
                    LoadChunk(loadType.RebuildMeshes, loadType.SyncVoxelsWithTerrain, chunk.Value);
                }
            }

            LoadChunksFromDir(loadType, new DirectoryInfo(path));
#else
            if (chunks == null) {
                Debug.LogError("Chunks dico should not be null in loading");
                return;
            }

            if (loadType.LoadVoxels) {
                foreach (var chunk in chunks) {
                    LoadChunk(loadType.RebuildMeshes, loadType.SyncVoxelsWithTerrain, chunk.Value);
                }
            }

            LoadChunksFromDir(loadType, new DirectoryInfo(PersistentRuntimePathData));
            LoadChunksFromStreamingAssetsDir(loadType);
#endif
        }

        private void LoadChunksFromDir(LoadType loadType, DirectoryInfo dir)
        {
            if (!dir.Exists)
                return;

            foreach (var file in dir.EnumerateFiles($"*.{VoxelFileExtension}")) {
                var chunkPosition = Chunk.GetPositionFromName(file.Name);
                LoadChunkFromFile(loadType, chunkPosition);
            }
        }

        private void LoadChunksFromStreamingAssetsDir(LoadType loadType)
        {
            if (chunksInStreamingAssets == null || chunksInStreamingAssets.Length == 0)
                return;

            Debug.Log($"Digger will now load {chunksInStreamingAssets.Length} chunks from StreamingAssets folder");

            foreach (var chunkPosition in chunksInStreamingAssets) {
                LoadChunkFromFile(loadType, chunkPosition);
            }
        }

        private void LoadChunkFromFile(LoadType loadType, Vector3i chunkPosition)
        {
            if (!chunks.ContainsKey(chunkPosition) && chunkPosition.x >= 0 && chunkPosition.z >= 0 &&
                chunkPosition.x <= TerrainChunkWidth && chunkPosition.z <= TerrainChunkHeight) {
                var chunkAlreadyExisted = GetOrCreateChunk(chunkPosition, out var chunk);
                if (loadType.LoadVoxels || !chunkAlreadyExisted) {
                    LoadChunk(loadType.RebuildMeshes || !chunkAlreadyExisted, loadType.SyncVoxelsWithTerrain, chunk);
                    if (!chunkAlreadyExisted)
                        Utils.D.Log(
                            $"Rebuilt mesh of chunk {chunk.ChunkPosition} because it was missing from dictionary");
                }
            }
        }

        private static void LoadChunk(bool rebuildMeshes, bool syncVoxelsWithTerrain, Chunk chunk)
        {
            var hasNewVoxelChunk = chunk.LoadVoxels(syncVoxelsWithTerrain);
            if (rebuildMeshes || hasNewVoxelChunk) {
                chunk.RebuildMeshes();
            }
        }

        public void UpdateStaticEditorFlags()
        {
            if (chunks == null)
                return;

            foreach (var chunk in chunks) {
                chunk.Value.UpdateStaticEditorFlags();
            }
        }

#if UNITY_EDITOR
        public void SaveMeshesAsAssets()
        {
            if (chunks == null)
                return;

            foreach (var chunk in chunks) {
                chunk.Value.SaveMeshesAsAssets();
            }
        }
#endif

        public void Clear()
        {
#if UNITY_EDITOR
            Utils.Profiler.BeginSample("[Dig] Clear");
            if (cutter != null) {
                cutter.Clear();
                cutter = null;
            }

            if (Directory.Exists(BasePathData)) {
                Directory.Delete(BasePathData, true);
                AssetDatabase.DeleteAsset(BasePathData);
            }

            if (chunks != null) {
                foreach (var chunk in chunks) {
                    if (Application.isEditor) {
                        DestroyImmediate(chunk.Value.gameObject);
                    } else {
                        Destroy(chunk.Value.gameObject);
                    }
                }

                chunks = null;
            }

            chunksToPersist = null;
            Materials = null;

            version = 1;
            PersistVersion();

            if (materialType == TerrainMaterialType.Standard || materialType == TerrainMaterialType.URP || materialType == TerrainMaterialType.HDRP) {
                if (GraphicsSettings.currentRenderPipeline != null) {
                    Terrain.materialTemplate = GraphicsSettings.currentRenderPipeline.defaultTerrainMaterial;
                } else {
                    Terrain.materialTemplate = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Terrain-Standard.mat");
                }
            }

            Undo.ClearAll();
            Utils.Profiler.EndSample();
#endif
        }

        public void CreateDirs()
        {
#if UNITY_EDITOR
            master.CreateDirs();
            basePathData = ComputeBasePathData();

            if (!Directory.Exists(BasePathData)) {
                AssetDatabase.CreateFolder(master.SceneDataPath, BaseFolder);
            }

            if (!Directory.Exists(InternalPathData)) {
                Directory.CreateDirectory(InternalPathData);
            }
#endif
        }


        #region DiggerPRO

        public void OnPreprocessBuild(bool includeVoxelData)
        {
#if UNITY_EDITOR
            if (Directory.Exists(StreamingAssetsPathData))
                Directory.Delete(StreamingAssetsPathData, true);

            chunksInStreamingAssets = null;

            if (includeVoxelData) {
                Directory.CreateDirectory(StreamingAssetsPathData);

                var chunksInStreamingAssetsList = new List<Vector3i>();
                foreach (var p in Directory.GetFiles(InternalPathData, $"*.{VoxelFileExtension}",
                                                     SearchOption.TopDirectoryOnly)) {
                    var fi = new FileInfo(p);
                    fi.CopyTo(Path.Combine(StreamingAssetsPathData, fi.Name));
                    chunksInStreamingAssetsList.Add(Chunk.GetPositionFromName(fi.Name));
                }

                foreach (var p in Directory.GetFiles(InternalPathData, $"*.{VoxelMetadataFileExtension}",
                                                     SearchOption.TopDirectoryOnly)) {
                    var fi = new FileInfo(p);
                    fi.CopyTo(Path.Combine(StreamingAssetsPathData, fi.Name));
                }

                chunksInStreamingAssets = chunksInStreamingAssetsList.ToArray();
            }
#endif
        }

        #endregion

#if UNITY_EDITOR
        private void OnDestroy()
        {
            if (Application.isEditor && Application.isPlaying) {
                if (cutter) {
                    cutter.OnExitPlayMode();
                }
            }
        }
#endif
    }
}