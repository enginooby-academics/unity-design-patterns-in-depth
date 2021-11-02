namespace Digger.TerrainCutters
{
    public struct CutEntry
    {
        public int X;
        public int Z;
        public byte RemoveDetailsOnly;

        public CutEntry(int x, int z, bool removeDetailsOnly)
        {
            X = x;
            Z = z;
            RemoveDetailsOnly = removeDetailsOnly ? (byte) 1 : (byte) 0;
        }
    }
}