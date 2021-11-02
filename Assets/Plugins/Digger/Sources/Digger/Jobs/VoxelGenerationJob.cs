using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Digger
{
    [BurstCompile(CompileSynchronously = true, FloatMode = FloatMode.Fast)]
    public struct VoxelGenerationJob : IJobParallelFor
    {
        public int ChunkAltitude;
        public int SizeVox;
        public int SizeVox2;

        [ReadOnly] [NativeDisableParallelForRestriction]
        public NativeArray<float> Heights;

        [WriteOnly] public NativeArray<Voxel> Voxels;

        public void Execute(int index)
        {
            var pi = Utils.IndexToXYZ(index, SizeVox, SizeVox2);
            var height = Heights[Utils.XYZToHeightIndex(pi, SizeVox)];
            var voxel = new Voxel(pi.y + ChunkAltitude - height);
            Voxels[index] = voxel;
        }
    }
}