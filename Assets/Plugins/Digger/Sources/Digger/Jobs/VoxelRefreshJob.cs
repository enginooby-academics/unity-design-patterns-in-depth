using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Digger
{
    [BurstCompile(CompileSynchronously = true, FloatMode = FloatMode.Fast)]
    public struct VoxelRefreshJob : IJobParallelFor
    {
        public int ChunkAltitude;
        public int SizeVox;
        public int SizeVox2;

        [ReadOnly] [NativeDisableParallelForRestriction]
        public NativeArray<float> Heights;

        [NativeDisableParallelForRestriction]
        public NativeArray<Voxel> Voxels;

        public void Execute(int index)
        {
            var pi = Utils.IndexToXYZ(index, SizeVox, SizeVox2);
            var height = Heights[Utils.XYZToHeightIndex(pi, SizeVox)];
            var voxel = Voxels[index];

            // Fix issue with previous versions of Digger (for retro-compatibility) where OnSurface was abusively set
            if (voxel.Alteration == Voxel.OnSurface && !Utils.IsOnSurface(pi, pi.y + ChunkAltitude, SizeVox, Heights)) {
                voxel.Alteration = Voxel.FarAboveSurface;
                voxel = Utils.AdjustAlteration(voxel, pi, ChunkAltitude, pi.y + ChunkAltitude - height, SizeVox, Heights);
                voxel = RetrieveTexture(pi, voxel);
            }

            if (!voxel.IsAlteredFarOrNearSurface) {
                voxel.Value = pi.y + ChunkAltitude - height;
            }

            Voxels[index] = voxel;
        }

        private Voxel RetrieveTexture(int3 pi, Voxel voxel)
        {
            for (var x = pi.x - 1; x <= pi.x + 1; ++x) {
                for (var y = pi.y - 1; y <= pi.y + 1; ++y) {
                    for (var z = pi.z - 1; z <= pi.z + 1; ++z) {
                        var neighbour = GetSafe(new int3(x, y, z));
                        if (neighbour.IsAlteredFarOrNearSurface) {
                            voxel.FirstTextureIndex = neighbour.FirstTextureIndex;
                            voxel.SecondTextureIndex = neighbour.SecondTextureIndex;
                            voxel.NormalizedTextureLerp = neighbour.NormalizedTextureLerp;
                            voxel.NormalizedLavaWeight = neighbour.NormalizedLavaWeight;
                            voxel.NormalizedPuddlesWeight = neighbour.NormalizedPuddlesWeight;
                            voxel.NormalizedStreamsWeight = neighbour.NormalizedStreamsWeight;
                            voxel.NormalizedWetnessWeight = neighbour.NormalizedWetnessWeight;
                            return voxel;
                        }
                    }
                }
            }

            return voxel;
        }

        private Voxel GetSafe(int3 pi)
        {
            if (pi.x < 0 || pi.x >= SizeVox ||
                pi.y < 0 || pi.y >= SizeVox ||
                pi.z < 0 || pi.z >= SizeVox)
                return new Voxel();
            return Voxels[pi.x * SizeVox2 + pi.y * SizeVox + pi.z];
        }
    }
}