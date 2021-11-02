namespace Digger
{
    public interface IHeightFeeder
    {
        float GetHeight(int x, int z);
        float GetVerticalNormal(int x, int z);
    }
}