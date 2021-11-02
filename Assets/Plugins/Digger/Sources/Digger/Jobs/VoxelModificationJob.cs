using System;
using Digger.TerrainCutters;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Digger
{
    [BurstCompile(CompileSynchronously = true, FloatMode = FloatMode.Fast)]
    public struct VoxelModificationJob : IJobParallelFor
    {
        public int SizeVox;
        public int SizeVox2;
        public int SizeOfMesh;
        public BrushType Brush;
        public ActionType Action;
        public float3 HeightmapScale;
        public float3 Center;
        public float ConeHeight;
        public bool UpsideDown;
        public float Radius;
        public float RadiusWithMargin;
        public float Intensity;
        public bool IsTargetIntensity;
        public int ChunkAltitude;
        public uint TextureIndex;
        public int2 CutSize;

        [ReadOnly] [NativeDisableParallelForRestriction]
        public NativeArray<float> Heights;

        public NativeArray<Voxel> Voxels;

        [WriteOnly] [NativeDisableParallelForRestriction] public NativeArray<int> Holes;

        private double coneAngle;
        private float upsideDownSign;

        public void PostConstruct()
        {
            if (ConeHeight > 0.1f)
                coneAngle = Math.Atan((double) Radius / ConeHeight);
            upsideDownSign = UpsideDown ? -1f : 1f;
        }

        public void Execute(int index)
        {
            var pi = Utils.IndexToXYZ(index, SizeVox, SizeVox2);
            var p = new float3(pi.x * HeightmapScale.x, pi.y, pi.z * HeightmapScale.z);
            var terrainHeight = Heights[Utils.XYZToHeightIndex(pi, SizeVox)];
            var terrainHeightValue = p.y + ChunkAltitude - terrainHeight;

            float2 distances;
            switch (Brush) {
                case BrushType.Sphere:
                    distances = ComputeSphereDistances(p);
                    break;
                case BrushType.HalfSphere:
                    distances = ComputeHalfSphereDistances(p);
                    break;
                case BrushType.RoundedCube:
                    distances = ComputeCubeDistances(p);
                    break;
                case BrushType.Stalagmite:
                    distances = ComputeConeDistances(p);
                    break;
                default:
                    return; // never happens
            }

            Voxel voxel;
            switch (Action) {
                case ActionType.Add:
                case ActionType.Dig:
                    var intensityWeight = Math.Max(1f, Math.Abs(terrainHeightValue) * 0.75f);
                    voxel = ApplyDigAdd(index, pi, Action == ActionType.Dig, distances, intensityWeight);
                    break;
                case ActionType.Paint:
                    voxel = ApplyPaint(index, distances.x);
                    break;
                case ActionType.PaintHoles:
                    voxel = ApplyPaintHoles(index, pi, distances.x);
                    break;
                case ActionType.Reset:
                    voxel = ApplyResetBrush(index, pi, p, terrainHeightValue);
                    break;
                default:
                    return; // never happens
            }


            if (voxel.Alteration != Voxel.Unaltered) {
                voxel = Utils.AdjustAlteration(voxel, pi, ChunkAltitude, terrainHeightValue, SizeVox, Heights);
            }

            if (voxel.IsAlteredNearBelowSurface || voxel.IsAlteredNearAboveSurface) {
                if (Action != ActionType.Reset) {
                    for (var z = -CutSize.y; z < CutSize.y; ++z) {
                        var pz = pi.z + z;
                        if (pz >= 0 && pz < SizeOfMesh) {
                            for (var x = -CutSize.x; x < CutSize.x; ++x) {
                                var px = pi.x + x;
                                if (px >= 0 && px < SizeOfMesh) {
                                    NativeCollections.Utils.IncrementAt(Holes, pz * SizeOfMesh + px);
                                }
                            }
                        }
                    }
                }
            }


            Voxels[index] = voxel;
        }

        private float2 ComputeSphereDistances(float3 p)
        {
            var vec = p - Center;
            var distance = (float) Math.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z);
            var flatDistance = (float) Math.Sqrt(vec.x * vec.x + vec.z * vec.z);
            return new float2(Radius - distance, RadiusWithMargin - flatDistance);
        }

        private float2 ComputeHalfSphereDistances(float3 p)
        {
            var vec = p - Center;
            var distance = (float) Math.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z);
            var flatDistance = (float) Math.Sqrt(vec.x * vec.x + vec.z * vec.z);
            return new float2(Math.Min(Radius - distance, vec.y), RadiusWithMargin - flatDistance);
        }

        private float2 ComputeCubeDistances(float3 p)
        {
            var vec = p - Center;
            var flatDistance = Math.Min(RadiusWithMargin - Math.Abs(vec.x), RadiusWithMargin - Math.Abs(vec.z));
            return new float2(
                Math.Min(Math.Min(Radius - Math.Abs(vec.x), Radius - Math.Abs(vec.y)), Radius - Math.Abs(vec.z)),
                flatDistance);
        }

        private float2 ComputeConeDistances(float3 p)
        {
            var coneVertex = Center + new float3(0, upsideDownSign * ConeHeight * 0.95f, 0);
            var vec = p - coneVertex;
            var distance = (float) Math.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z);
            var flatDistance = (float) Math.Sqrt(vec.x * vec.x + vec.z * vec.z);
            var pointAngle = Math.Asin((double) flatDistance / distance);
            var d = -distance * Math.Sin(Math.Abs(pointAngle - coneAngle)) * Math.Sign(pointAngle - coneAngle);
            return new float2(
                Math.Min(Math.Min((float) d, ConeHeight + upsideDownSign * vec.y), -upsideDownSign * vec.y),
                RadiusWithMargin - flatDistance);
        }

        private Voxel ApplyDigAdd(int index, int3 pi, bool dig, float2 distances, float intensityWeight)
        {
            var voxel = Voxels[index];
            var currentValF = voxel.Value;

            if (dig) {
                voxel.Value = Math.Max(currentValF, currentValF + Intensity * intensityWeight * distances.x);
            } else {
                voxel.Value = Math.Min(currentValF, currentValF - Intensity * intensityWeight * distances.x);
            }

            if (distances.x >= 0) {
                voxel.Alteration = Voxel.FarAboveSurface;
                voxel.AddTexture(TextureIndex, 1f);
            } else if (distances.y > 0 && voxel.Alteration == Voxel.Unaltered && Utils.IsOnSurface(pi, pi.y + ChunkAltitude, SizeVox, Heights)) {
                voxel.Alteration = Voxel.OnSurface;
            }

            return voxel;
        }

        private Voxel ApplyPaint(int index, float distance)
        {
            var voxel = Voxels[index];

            if (distance >= 0) {
                if (IsTargetIntensity) {
                    if (TextureIndex < 28) {
                        voxel.SetTexture(TextureIndex, Intensity);
                    } else if (TextureIndex == 28) {
                        voxel.NormalizedWetnessWeight = Intensity;
                    } else if (TextureIndex == 29) {
                        voxel.NormalizedPuddlesWeight = Intensity;
                    } else if (TextureIndex == 30) {
                        voxel.NormalizedStreamsWeight = Intensity;
                    } else if (TextureIndex == 31) {
                        voxel.NormalizedLavaWeight = Intensity;
                    }
                } else {
                    if (TextureIndex < 28) {
                        voxel.AddTexture(TextureIndex, Intensity);
                    } else if (TextureIndex == 28) {
                        voxel.NormalizedWetnessWeight += Intensity;
                    } else if (TextureIndex == 29) {
                        voxel.NormalizedPuddlesWeight += Intensity;
                    } else if (TextureIndex == 30) {
                        voxel.NormalizedStreamsWeight += Intensity;
                    } else if (TextureIndex == 31) {
                        voxel.NormalizedLavaWeight += Intensity;
                    }
                }
            }

            return voxel;
        }

        private Voxel ApplyPaintHoles(int index, int3 pi, float distance)
        {
            var voxel = Voxels[index];
            if (distance >= 0 && Intensity > 0 && voxel.Alteration != Voxel.Unaltered) {
                voxel.Alteration = Voxel.Hole;
            } else if (distance >= 0 && Intensity < 0 && voxel.Alteration == Voxel.Hole) {
                var onSurface = Utils.IsOnSurface(pi, pi.y + ChunkAltitude, SizeVox, Heights);
                voxel.Alteration = onSurface ? Voxel.OnSurface : Voxel.FarAboveSurface;
            }

            return voxel;
        }

        private Voxel ApplyResetBrush(int index, int3 pi, float3 p, float terrainHeightValue)
        {
            var vec = p - Center;
            var flatDistance = (float) Math.Sqrt(vec.x * vec.x + vec.z * vec.z);
            if (flatDistance <= RadiusWithMargin) {
                var voxel = Voxels[index];
                if (voxel.Alteration == Voxel.Unaltered || flatDistance < Radius) {
                    voxel.Alteration = Voxel.Unaltered;
                    var px = pi.x;
                    var pz = pi.z;
                    if (px >= 0 && px < SizeOfMesh && pz >= 0 && pz < SizeOfMesh) {
                        NativeCollections.Utils.IncrementAt(Holes, pz * SizeOfMesh + px);
                    }
                } else {
                    voxel.Alteration = Voxel.OnSurface;
                }

                voxel.Value = terrainHeightValue;
                return voxel;
            }

            return Voxels[index];
        }
    }
}