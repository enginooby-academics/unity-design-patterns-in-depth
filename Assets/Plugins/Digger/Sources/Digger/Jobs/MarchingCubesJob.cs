using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Digger
{
    [BurstCompile(CompileSynchronously = true, FloatMode = FloatMode.Fast)]
    internal struct MarchingCubesJob : IJobParallelFor
    {
        public struct Out
        {
            public NativeArray<Vector3> outVertices;
            public NativeArray<Vector3> outNormals;
            public NativeArray<uint> outInfos;
            public NativeArray<int> outTriangles;
            public NativeArray<Vector2> outUV1s;
            public NativeArray<Color> outColors;
            public NativeArray<Vector4> outUV2s;
            public NativeArray<Vector4> outUV3s;
            public NativeArray<Vector4> outUV4s;

            public void Dispose()
            {
                outVertices.Dispose();
                outNormals.Dispose();
                outInfos.Dispose();
                outTriangles.Dispose();
                outUV1s.Dispose();
                outColors.Dispose();
                outUV2s.Dispose();
                outUV3s.Dispose();
                outUV4s.Dispose();
            }

            public static Out New()
            {
                return new Out
                {
                    outVertices = new NativeArray<Vector3>(65532, Allocator.Persistent,
                                                           NativeArrayOptions.UninitializedMemory),
                    outNormals = new NativeArray<Vector3>(65532, Allocator.Persistent,
                                                          NativeArrayOptions.UninitializedMemory),
                    outTriangles = new NativeArray<int>(65532, Allocator.Persistent,
                                                        NativeArrayOptions.UninitializedMemory),
                    outInfos = new NativeArray<uint>(65532, Allocator.Persistent,
                                                     NativeArrayOptions.UninitializedMemory),
                    outUV1s = new NativeArray<Vector2>(65532, Allocator.Persistent,
                                                       NativeArrayOptions.UninitializedMemory),
                    outColors = new NativeArray<Color>(65532, Allocator.Persistent,
                                                       NativeArrayOptions.UninitializedMemory),
                    outUV2s = new NativeArray<Vector4>(65532, Allocator.Persistent,
                                                       NativeArrayOptions.UninitializedMemory),
                    outUV3s = new NativeArray<Vector4>(65532, Allocator.Persistent,
                                                       NativeArrayOptions.UninitializedMemory),
                    outUV4s = new NativeArray<Vector4>(65532, Allocator.Persistent,
                                                       NativeArrayOptions.UninitializedMemory)
                };
            }
        }

        private struct WorkNorm
        {
            public Vector3 N0;
            public Vector3 N1;
            public Vector3 N2;
            public Vector3 N3;
            public Vector3 N4;
            public Vector3 N5;
            public Vector3 N6;
            public Vector3 N7;

            public Vector3 this[int i] {
                get {
                    switch (i) {
                        case 0: return N0;
                        case 1: return N1;
                        case 2: return N2;
                        case 3: return N3;
                        case 4: return N4;
                        case 5: return N5;
                        case 6: return N6;
                        case 7: return N7;
                    }

                    return Vector3.zero; // don't throw an exception to allow Burst compilation
                }
            }
        }

        private struct WorkVert
        {
            public Vector3 V0;
            public Vector3 V1;
            public Vector3 V2;
            public Vector3 V3;
            public Vector3 V4;
            public Vector3 V5;
            public Vector3 V6;
            public Vector3 V7;
            public Vector3 V8;
            public Vector3 V9;
            public Vector3 V10;
            public Vector3 V11;

            public Vector3 this[int i] {
                get {
                    switch (i) {
                        case 0:  return V0;
                        case 1:  return V1;
                        case 2:  return V2;
                        case 3:  return V3;
                        case 4:  return V4;
                        case 5:  return V5;
                        case 6:  return V6;
                        case 7:  return V7;
                        case 8:  return V8;
                        case 9:  return V9;
                        case 10: return V10;
                        case 11: return V11;
                    }

                    return Vector3.zero; // don't throw an exception to allow Burst compilation
                }
            }
        }

        private struct WorkVertIndices
        {
            public int Vi0;
            public int Vi1;
            public int Vi2;
            public int Vi3;
            public int Vi4;
            public int Vi5;
            public int Vi6;
            public int Vi7;
            public int Vi8;
            public int Vi9;
            public int Vi10;
            public int Vi11;

            public int this[int i] {
                get {
                    switch (i) {
                        case 0:  return Vi0;
                        case 1:  return Vi1;
                        case 2:  return Vi2;
                        case 3:  return Vi3;
                        case 4:  return Vi4;
                        case 5:  return Vi5;
                        case 6:  return Vi6;
                        case 7:  return Vi7;
                        case 8:  return Vi8;
                        case 9:  return Vi9;
                        case 10: return Vi10;
                        case 11: return Vi11;
                    }

                    return -1; // don't throw an exception to allow Burst compilation
                }
            }
        }

        public int SizeVox;
        public int SizeVox2;

        [ReadOnly] [NativeDisableParallelForRestriction]
        private NativeArray<int> edgeTable;

        [ReadOnly] [NativeDisableParallelForRestriction]
        private NativeArray<int> triTable;

        [ReadOnly] [NativeDisableParallelForRestriction]
        private NativeArray<Vector3> corners;

        [ReadOnly] [NativeDisableParallelForRestriction]
        private NativeArray<Voxel> voxels;

        [ReadOnly] [NativeDisableParallelForRestriction]
        private NativeArray<float> alphamaps;

        private int2 alphamapsSize;
        private int3 localAlphamapsSize;

        private NativeCollections.NativeCounter.Concurrent vertexCounter;
        private NativeCollections.NativeCounter.Concurrent triangleCounter;

        [WriteOnly] [NativeDisableParallelForRestriction]
        private NativeArray<Vector3> outVertices;

        [WriteOnly] [NativeDisableParallelForRestriction]
        private NativeArray<Vector3> outNormals;

        [WriteOnly] [NativeDisableParallelForRestriction]
        private NativeArray<int> outTriangles;

        [WriteOnly] [NativeDisableParallelForRestriction]
        private NativeArray<uint> outInfos;

        [WriteOnly] [NativeDisableParallelForRestriction]
        private NativeArray<Vector2> outUV1s;

        [WriteOnly] [NativeDisableParallelForRestriction]
        private NativeArray<Color> outColors;

        [WriteOnly] [NativeDisableParallelForRestriction]
        private NativeArray<Vector4> outUV2s;

        [WriteOnly] [NativeDisableParallelForRestriction]
        private NativeArray<Vector4> outUV3s;

        [WriteOnly] [NativeDisableParallelForRestriction]
        private NativeArray<Vector4> outUV4s;


        private Vector3 chunkWorldPosition;
        private Vector3 scale;
        private Vector2 uvScale;
        private int2 alphamapOrigin;
        private int lod;
        private TerrainMaterialType materialType;

        public float Isovalue;
        public byte AlteredOnly;
        public byte FullOutput;


        public MarchingCubesJob(NativeArray<int> edgeTable,
                                NativeArray<int> triTable,
                                NativeArray<Vector3> corners,
                                NativeCollections.NativeCounter.Concurrent vertexCounter,
                                NativeCollections.NativeCounter.Concurrent triangleCounter,
                                NativeArray<Voxel> voxels,
                                NativeArray<float> alphamaps,
                                Out o,
                                Vector3 scale,
                                Vector2 uvScale,
                                Vector3 chunkWorldPosition,
                                int lod,
                                int2 alphamapOrigin,
                                int2 alphamapsSize,
                                int3 localAlphamapsSize,
                                TerrainMaterialType materialType)
        {
            this.edgeTable = edgeTable;
            this.triTable = triTable;
            this.corners = corners;
            this.vertexCounter = vertexCounter;
            this.triangleCounter = triangleCounter;
            this.voxels = voxels;
            this.alphamaps = alphamaps;

            this.outVertices = o.outVertices;
            this.outNormals = o.outNormals;
            this.outInfos = o.outInfos;
            this.outTriangles = o.outTriangles;
            this.outUV1s = o.outUV1s;
            this.outColors = o.outColors;
            this.outUV2s = o.outUV2s;
            this.outUV3s = o.outUV3s;
            this.outUV4s = o.outUV4s;

            this.SizeVox = 0;
            this.SizeVox2 = 0;
            this.Isovalue = 0;
            this.AlteredOnly = 1;
            this.FullOutput = 1;
            this.scale = scale;
            this.lod = lod;
            this.alphamapsSize = alphamapsSize;
            this.localAlphamapsSize = localAlphamapsSize;
            this.uvScale = uvScale;
            this.alphamapOrigin = alphamapOrigin;
            this.chunkWorldPosition = chunkWorldPosition;
            this.materialType = materialType;
        }

        private static Voxel VertexInfo(Voxel vA, Voxel vB)
        {
            // Use 'Altered' value of the most altered voxel to avoid artifacts (ie. prefer voxel which has been actually altered by VoxelModificationJob over a "virgin" voxel)
            if (vA.Alteration > vB.Alteration)
                return vA;

            if (vA.Alteration < vB.Alteration)
                return vB;

            if (math.abs(vA.Value) < math.abs(vB.Value))
                return vA;

            return vB;
        }

        private Vector3 VertexInterp(Vector3 p1, Vector3 p2, Voxel vA, Voxel vB)
        {
            if (Utils.Approximately(vA.Value, 0))
                return p1;
            if (Utils.Approximately(vB.Value, 0))
                return p2;

            var mu = (Isovalue - vA.Value) / (vB.Value - vA.Value);

            Vector3 p;
            p.x = p1.x + mu * (p2.x - p1.x);
            p.y = p1.y + mu * (p2.y - p1.y);
            p.z = p1.z + mu * (p2.z - p1.z);

            return p;
        }

        private Vector3 ComputeNormalAt(int xi, int yi, int zi, float voxelOriginValue)
        {
            return Utils.Normalize(new Vector3(
                                       voxels[(xi + 1) * SizeVox2 + yi * SizeVox + zi].Value - voxelOriginValue,
                                       voxels[xi * SizeVox2 + (yi + 1) * SizeVox + zi].Value - voxelOriginValue,
                                       voxels[xi * SizeVox2 + yi * SizeVox + (zi + 1)].Value - voxelOriginValue
                                   ));
        }


        private void ComputeUVsAndColor(int vertIndex, Vector3 vertex, Voxel voxel)
        {
            if (materialType == TerrainMaterialType.MicroSplat) {
                ComputeUVsAndColorForMicroSplat(vertIndex, vertex, voxel);
                return;
            }

            var uv = new Vector2((chunkWorldPosition.x + vertex.x) * uvScale.x,
                                 (chunkWorldPosition.z + vertex.z) * uvScale.y);
            outUV1s[vertIndex] = uv;

            if (voxel.Alteration == Voxel.Unaltered || voxel.Alteration == Voxel.OnSurface) {
                // near the terrain surface -> set same texture
                outColors[vertIndex] = GetControlAt(uv, 0);
                outUV2s[vertIndex] = GetControlAt(uv, 1);
                outUV3s[vertIndex] = GetControlAt(uv, 2);
                outUV4s[vertIndex] = GetControlAt(uv, 3);
            } else {
                var firstTextureIndex = voxel.FirstTextureIndex;
                var secondTextureIndex = voxel.SecondTextureIndex;
                var lerp = voxel.NormalizedTextureLerp;
                outColors[vertIndex] = GetControlFor(firstTextureIndex, secondTextureIndex, lerp, 0);
                outUV2s[vertIndex] = GetControlFor(firstTextureIndex, secondTextureIndex, lerp, 1);
                outUV3s[vertIndex] = GetControlFor(firstTextureIndex, secondTextureIndex, lerp, 2);
                outUV4s[vertIndex] = GetControlFor(firstTextureIndex, secondTextureIndex, lerp, 3);
            }
        }

        private void ComputeUVsAndColorForMicroSplat(int vertIndex, Vector3 vertex, Voxel voxel)
        {
            var uv = new Vector2((chunkWorldPosition.x + vertex.x) * uvScale.x,
                                 (chunkWorldPosition.z + vertex.z) * uvScale.y);
            outUV1s[vertIndex] = uv;

            if (voxel.Alteration == Voxel.Unaltered || voxel.Alteration == Voxel.OnSurface) {
                // near the terrain surface -> set same texture
                outColors[vertIndex] = new Color(
                    EncodeToFloat(GetControlAt(uv, 0)),
                    EncodeToFloat(GetControlAt(uv, 1)),
                    EncodeToFloat(GetControlAt(uv, 2)),
                    EncodeToFloat(GetControlAt(uv, 3))
                );
                outUV2s[vertIndex] = new Vector4(
                    0,
                    0,
                    EncodeToFloat(GetControlAt(uv, 4)),
                    EncodeToFloat(GetControlAt(uv, 5))
                );
                outUV3s[vertIndex] = new Vector4(
                    0,
                    0,
                    EncodeToFloat(GetControlAt(uv, 6)),
                    EncodeToFloat(GetControlAt(uv, 7))
                );
            } else {
                var firstTextureIndex = voxel.FirstTextureIndex;
                var secondTextureIndex = voxel.SecondTextureIndex;
                var lerp = voxel.NormalizedTextureLerp;

                outColors[vertIndex] = new Color(
                    EncodeToFloat(GetControlFor(firstTextureIndex, secondTextureIndex, lerp, 0)),
                    EncodeToFloat(GetControlFor(firstTextureIndex, secondTextureIndex, lerp, 1)),
                    EncodeToFloat(GetControlFor(firstTextureIndex, secondTextureIndex, lerp, 2)),
                    EncodeToFloat(GetControlFor(firstTextureIndex, secondTextureIndex, lerp, 3))
                );
                outUV2s[vertIndex] = new Vector4(
                    0,
                    0,
                    EncodeToFloat(GetControlFor(firstTextureIndex, secondTextureIndex, lerp, 4)),
                    EncodeToFloat(GetControlFor(firstTextureIndex, secondTextureIndex, lerp, 5))
                );
                outUV3s[vertIndex] = new Vector4(
                    0,
                    0,
                    EncodeToFloat(GetControlFor(firstTextureIndex, secondTextureIndex, lerp, 6)),
                    EncodeToFloat(new Vector4(voxel.NormalizedWetnessWeight, voxel.NormalizedPuddlesWeight,
                                              voxel.NormalizedStreamsWeight, voxel.NormalizedLavaWeight))
                );
            }
        }

        private Vector4 GetControlAt(Vector2 uv, int index)
        {
            // adjust splatUVs so the edges of the terrain tile lie on pixel centers
            var splatUV = new Vector2(uv.x * (alphamapsSize.x - 1), uv.y * (alphamapsSize.y - 1));

            var wx = math.clamp(Convert.ToInt32(math.floor(splatUV.x)), 0, alphamapsSize.x - 2);
            var wz = math.clamp(Convert.ToInt32(math.floor(splatUV.y)), 0, alphamapsSize.y - 2);
            var relPos = splatUV - new Vector2(wx, wz);
            var x = math.clamp(wx - alphamapOrigin.x, 0, localAlphamapsSize.x - 2);
            var z = math.clamp(wz - alphamapOrigin.y, 0, localAlphamapsSize.y - 2);

            index *= 4;

            var mapCount = localAlphamapsSize.z;
            var ctrl = Vector4.zero;
            for (var i = 0; i < 4; ++i) {
                if (index + i < mapCount) {
                    var a00 = alphamaps[x * localAlphamapsSize.y * mapCount + z * mapCount + index + i];
                    var a01 = alphamaps[(x + 1) * localAlphamapsSize.y * mapCount + z * mapCount + index + i];
                    var a10 = alphamaps[x * localAlphamapsSize.y * mapCount + (z + 1) * mapCount + index + i];
                    var a11 = alphamaps[(x + 1) * localAlphamapsSize.y * mapCount + (z + 1) * mapCount + index + i];
                    ctrl[i] = Utils.BilinearInterpolate(a00, a01, a10, a11, relPos.y, relPos.x);
                }
            }

            return ctrl;
        }

        private static Vector4 GetControlFor(uint firstTextureIndex, uint secondTextureIndex, float lerp, int index)
        {
            var ctrl = Vector4.zero;
            if (index * 4 == firstTextureIndex)
                ctrl.x = 1f - lerp;
            else if (index * 4 == secondTextureIndex)
                ctrl.x = lerp;

            if (index * 4 + 1 == firstTextureIndex)
                ctrl.y = 1f - lerp;
            else if (index * 4 + 1 == secondTextureIndex)
                ctrl.y = lerp;

            if (index * 4 + 2 == firstTextureIndex)
                ctrl.z = 1f - lerp;
            else if (index * 4 + 2 == secondTextureIndex)
                ctrl.z = lerp;

            if (index * 4 + 3 == firstTextureIndex)
                ctrl.w = 1f - lerp;
            else if (index * 4 + 3 == secondTextureIndex)
                ctrl.w = lerp;

            return ctrl;
        }

        private static float EncodeToFloat(Vector4 enc)
        {
            var ex = (uint) (enc.x * 255);
            var ey = (uint) (enc.y * 255);
            var ez = (uint) (enc.z * 255);
            var ew = (uint) (enc.w * 255);
            var v = (ex << 24) + (ey << 16) + (ez << 8) + ew;
            return v / (256.0f * 256.0f * 256.0f * 256.0f);
        }


        public void Execute(int index)
        {
            var pi = Utils.IndexToXYZ(index, SizeVox, SizeVox2);

            if (pi.x >= SizeVox - lod - 1 ||
                pi.y >= SizeVox - lod - 1 ||
                pi.z >= SizeVox - lod - 1 ||
                pi.x % lod != 0 || pi.y % lod != 0 || pi.z % lod != 0)
                return;

            var v0 = voxels[pi.x * SizeVox * SizeVox + pi.y * SizeVox + pi.z];
            var v1 = voxels[(pi.x + lod) * SizeVox * SizeVox + pi.y * SizeVox + pi.z];
            var v2 = voxels[(pi.x + lod) * SizeVox * SizeVox + pi.y * SizeVox + (pi.z + lod)];
            var v3 = voxels[pi.x * SizeVox * SizeVox + pi.y * SizeVox + (pi.z + lod)];
            var v4 = voxels[pi.x * SizeVox * SizeVox + (pi.y + lod) * SizeVox + pi.z];
            var v5 = voxels[(pi.x + lod) * SizeVox * SizeVox + (pi.y + lod) * SizeVox + pi.z];
            var v6 = voxels[(pi.x + lod) * SizeVox * SizeVox + (pi.y + lod) * SizeVox + (pi.z + lod)];
            var v7 = voxels[pi.x * SizeVox * SizeVox + (pi.y + lod) * SizeVox + (pi.z + lod)];

            var alt0 = v0.Alteration;
            var alt1 = v1.Alteration;
            var alt2 = v2.Alteration;
            var alt3 = v3.Alteration;
            var alt4 = v4.Alteration;
            var alt5 = v5.Alteration;
            var alt6 = v6.Alteration;
            var alt7 = v7.Alteration;
            if (alt0 == Voxel.Hole ||
                alt1 == Voxel.Hole ||
                alt2 == Voxel.Hole ||
                alt3 == Voxel.Hole ||
                alt4 == Voxel.Hole ||
                alt5 == Voxel.Hole ||
                alt6 == Voxel.Hole ||
                alt7 == Voxel.Hole)
                return;

            if (AlteredOnly == 1) {
                if (alt0 == Voxel.Unaltered &&
                    alt1 == Voxel.Unaltered &&
                    alt2 == Voxel.Unaltered &&
                    alt3 == Voxel.Unaltered &&
                    alt4 == Voxel.Unaltered &&
                    alt5 == Voxel.Unaltered &&
                    alt6 == Voxel.Unaltered &&
                    alt7 == Voxel.Unaltered)
                    return;
            }

            var cubeindex = 0;
            if (v0.IsInside) cubeindex |= 1;
            if (v1.IsInside) cubeindex |= 2;
            if (v2.IsInside) cubeindex |= 4;
            if (v3.IsInside) cubeindex |= 8;
            if (v4.IsInside) cubeindex |= 16;
            if (v5.IsInside) cubeindex |= 32;
            if (v6.IsInside) cubeindex |= 64;
            if (v7.IsInside) cubeindex |= 128;

            /* Cube is entirely in/out of the surface */
            if (cubeindex == 0 || cubeindex == 255)
                return;

            var position = new Vector3
            {
                x = pi.x,
                y = pi.y,
                z = pi.z
            };

            var voxelNorm = new WorkNorm
            {
                N0 = ComputeNormalAt(pi.x, pi.y, pi.z, v0.Value),
                N1 = ComputeNormalAt((pi.x + lod), pi.y, pi.z, v1.Value),
                N2 = ComputeNormalAt((pi.x + lod), pi.y, (pi.z + lod), v2.Value),
                N3 = ComputeNormalAt(pi.x, pi.y, (pi.z + lod), v3.Value),
                N4 = ComputeNormalAt(pi.x, (pi.y + lod), pi.z, v4.Value),
                N5 = ComputeNormalAt((pi.x + lod), (pi.y + lod), pi.z, v5.Value),
                N6 = ComputeNormalAt((pi.x + lod), (pi.y + lod), (pi.z + lod), v6.Value),
                N7 = ComputeNormalAt(pi.x, (pi.y + lod), (pi.z + lod), v7.Value)
            };

            var wVert = new WorkVert();
            var wVertIndices = new WorkVertIndices();

            /* Find the vertices where the surface intersects the cube */
            if ((edgeTable[cubeindex] & 1) != 0) {
                var norm = VertexInterp(voxelNorm.N0, voxelNorm.N1, v0, v1);
                wVert.V0 = Vector3.Scale(position + VertexInterp(corners[0], corners[1], v0, v1) * lod, scale);
                var vertIndex = vertexCounter.Increment() - 1;
                outVertices[vertIndex] = wVert.V0;
                outNormals[vertIndex] = Utils.Normalize(norm);
                wVertIndices.Vi0 = vertIndex;
                if (FullOutput == 1) {
                    var vox = VertexInfo(v0, v1);
                    outInfos[vertIndex] = vox.Alteration;
                    ComputeUVsAndColor(vertIndex, wVert.V0, vox);
                }
            }

            if ((edgeTable[cubeindex] & 2) != 0) {
                var norm = VertexInterp(voxelNorm.N1, voxelNorm.N2, v1, v2);
                wVert.V1 = Vector3.Scale(position + VertexInterp(corners[1], corners[2], v1, v2) * lod, scale);
                var vertIndex = vertexCounter.Increment() - 1;
                outVertices[vertIndex] = wVert.V1;
                outNormals[vertIndex] = Utils.Normalize(norm);
                wVertIndices.Vi1 = vertIndex;
                if (FullOutput == 1) {
                    var vox = VertexInfo(v1, v2);
                    outInfos[vertIndex] = vox.Alteration;
                    ComputeUVsAndColor(vertIndex, wVert.V1, vox);
                }
            }

            if ((edgeTable[cubeindex] & 4) != 0) {
                var norm = VertexInterp(voxelNorm.N2, voxelNorm.N3, v2, v3);
                wVert.V2 = Vector3.Scale(position + VertexInterp(corners[2], corners[3], v2, v3) * lod, scale);
                var vertIndex = vertexCounter.Increment() - 1;
                outVertices[vertIndex] = wVert.V2;
                outNormals[vertIndex] = Utils.Normalize(norm);
                wVertIndices.Vi2 = vertIndex;
                if (FullOutput == 1) {
                    var vox = VertexInfo(v2, v3);
                    outInfos[vertIndex] = vox.Alteration;
                    ComputeUVsAndColor(vertIndex, wVert.V2, vox);
                }
            }

            if ((edgeTable[cubeindex] & 8) != 0) {
                var norm = VertexInterp(voxelNorm.N3, voxelNorm.N0, v3, v0);
                wVert.V3 = Vector3.Scale(position + VertexInterp(corners[3], corners[0], v3, v0) * lod, scale);
                var vertIndex = vertexCounter.Increment() - 1;
                outVertices[vertIndex] = wVert.V3;
                outNormals[vertIndex] = Utils.Normalize(norm);
                wVertIndices.Vi3 = vertIndex;
                if (FullOutput == 1) {
                    var vox = VertexInfo(v3, v0);
                    outInfos[vertIndex] = vox.Alteration;
                    ComputeUVsAndColor(vertIndex, wVert.V3, vox);
                }
            }

            if ((edgeTable[cubeindex] & 16) != 0) {
                var norm = VertexInterp(voxelNorm.N4, voxelNorm.N5, v4, v5);
                wVert.V4 = Vector3.Scale(position + VertexInterp(corners[4], corners[5], v4, v5) * lod, scale);
                var vertIndex = vertexCounter.Increment() - 1;
                outVertices[vertIndex] = wVert.V4;
                outNormals[vertIndex] = Utils.Normalize(norm);
                wVertIndices.Vi4 = vertIndex;
                if (FullOutput == 1) {
                    var vox = VertexInfo(v4, v5);
                    outInfos[vertIndex] = vox.Alteration;
                    ComputeUVsAndColor(vertIndex, wVert.V4, vox);
                }
            }

            if ((edgeTable[cubeindex] & 32) != 0) {
                var norm = VertexInterp(voxelNorm.N5, voxelNorm.N6, v5, v6);
                wVert.V5 = Vector3.Scale(position + VertexInterp(corners[5], corners[6], v5, v6) * lod, scale);
                var vertIndex = vertexCounter.Increment() - 1;
                outVertices[vertIndex] = wVert.V5;
                outNormals[vertIndex] = Utils.Normalize(norm);
                wVertIndices.Vi5 = vertIndex;
                if (FullOutput == 1) {
                    var vox = VertexInfo(v5, v6);
                    outInfos[vertIndex] = vox.Alteration;
                    ComputeUVsAndColor(vertIndex, wVert.V5, vox);
                }
            }

            if ((edgeTable[cubeindex] & 64) != 0) {
                var norm = VertexInterp(voxelNorm.N6, voxelNorm.N7, v6, v7);
                wVert.V6 = Vector3.Scale(position + VertexInterp(corners[6], corners[7], v6, v7) * lod, scale);
                var vertIndex = vertexCounter.Increment() - 1;
                outVertices[vertIndex] = wVert.V6;
                outNormals[vertIndex] = Utils.Normalize(norm);
                wVertIndices.Vi6 = vertIndex;
                if (FullOutput == 1) {
                    var vox = VertexInfo(v6, v7);
                    outInfos[vertIndex] = vox.Alteration;
                    ComputeUVsAndColor(vertIndex, wVert.V6, vox);
                }
            }

            if ((edgeTable[cubeindex] & 128) != 0) {
                var norm = VertexInterp(voxelNorm.N7, voxelNorm.N4, v7, v4);
                wVert.V7 = Vector3.Scale(position + VertexInterp(corners[7], corners[4], v7, v4) * lod, scale);
                var vertIndex = vertexCounter.Increment() - 1;
                outVertices[vertIndex] = wVert.V7;
                outNormals[vertIndex] = Utils.Normalize(norm);
                wVertIndices.Vi7 = vertIndex;
                if (FullOutput == 1) {
                    var vox = VertexInfo(v7, v4);
                    outInfos[vertIndex] = vox.Alteration;
                    ComputeUVsAndColor(vertIndex, wVert.V7, vox);
                }
            }

            if ((edgeTable[cubeindex] & 256) != 0) {
                var norm = VertexInterp(voxelNorm.N0, voxelNorm.N4, v0, v4);
                wVert.V8 = Vector3.Scale(position + VertexInterp(corners[0], corners[4], v0, v4) * lod, scale);
                var vertIndex = vertexCounter.Increment() - 1;
                outVertices[vertIndex] = wVert.V8;
                outNormals[vertIndex] = Utils.Normalize(norm);
                wVertIndices.Vi8 = vertIndex;
                if (FullOutput == 1) {
                    var vox = VertexInfo(v0, v4);
                    outInfos[vertIndex] = vox.Alteration;
                    ComputeUVsAndColor(vertIndex, wVert.V8, vox);
                }
            }

            if ((edgeTable[cubeindex] & 512) != 0) {
                var norm = VertexInterp(voxelNorm.N1, voxelNorm.N5, v1, v5);
                wVert.V9 = Vector3.Scale(position + VertexInterp(corners[1], corners[5], v1, v5) * lod, scale);
                var vertIndex = vertexCounter.Increment() - 1;
                outVertices[vertIndex] = wVert.V9;
                outNormals[vertIndex] = Utils.Normalize(norm);
                wVertIndices.Vi9 = vertIndex;
                if (FullOutput == 1) {
                    var vox = VertexInfo(v1, v5);
                    outInfos[vertIndex] = vox.Alteration;
                    ComputeUVsAndColor(vertIndex, wVert.V9, vox);
                }
            }

            if ((edgeTable[cubeindex] & 1024) != 0) {
                var norm = VertexInterp(voxelNorm.N2, voxelNorm.N6, v2, v6);
                wVert.V10 = Vector3.Scale(position + VertexInterp(corners[2], corners[6], v2, v6) * lod, scale);
                var vertIndex = vertexCounter.Increment() - 1;
                outVertices[vertIndex] = wVert.V10;
                outNormals[vertIndex] = Utils.Normalize(norm);
                wVertIndices.Vi10 = vertIndex;
                if (FullOutput == 1) {
                    var vox = VertexInfo(v2, v6);
                    outInfos[vertIndex] = vox.Alteration;
                    ComputeUVsAndColor(vertIndex, wVert.V10, vox);
                }
            }

            if ((edgeTable[cubeindex] & 2048) != 0) {
                var norm = VertexInterp(voxelNorm.N3, voxelNorm.N7, v3, v7);
                wVert.V11 = Vector3.Scale(position + VertexInterp(corners[3], corners[7], v3, v7) * lod, scale);
                var vertIndex = vertexCounter.Increment() - 1;
                outVertices[vertIndex] = wVert.V11;
                outNormals[vertIndex] = Utils.Normalize(norm);
                wVertIndices.Vi11 = vertIndex;
                if (FullOutput == 1) {
                    var vox = VertexInfo(v3, v7);
                    outInfos[vertIndex] = vox.Alteration;
                    ComputeUVsAndColor(vertIndex, wVert.V11, vox);
                }
            }

            /* Create the triangle */
            for (var i = 0; triTable[cubeindex * 16 + i] != -1; i += 3) {
                var i1 = triTable[cubeindex * 16 + (i + 0)];
                var i2 = triTable[cubeindex * 16 + (i + 1)];
                var i3 = triTable[cubeindex * 16 + (i + 2)];
                var vert1 = wVert[i1];
                var vert2 = wVert[i2];
                var vert3 = wVert[i3];
                if (!Utils.Approximately(vert1, vert2) &&
                    !Utils.Approximately(vert2, vert3) &&
                    !Utils.Approximately(vert1, vert3) &&
                    !Utils.AreColinear(vert1, vert2, vert3)) {
                    var triIndex = triangleCounter.Increment() - 3;
                    outTriangles[triIndex + 0] = wVertIndices[i1];
                    outTriangles[triIndex + 1] = wVertIndices[i2];
                    outTriangles[triIndex + 2] = wVertIndices[i3];
                }
            }
        }
    }
}