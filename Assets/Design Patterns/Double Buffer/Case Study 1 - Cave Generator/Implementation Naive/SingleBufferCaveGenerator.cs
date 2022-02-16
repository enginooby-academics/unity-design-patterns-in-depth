namespace DoubleBufferPattern.Case1.Naive {
  public class SingleBufferCaveGenerator : CaveGenerator {
    private int[,] _buffer;

    private void Start() {
      _buffer = new int[GRID_SIZE, GRID_SIZE];
      InitBuffer(_buffer);
      StartCoroutine(SimulateCavePatternCoroutine());
    }

    protected override void WriteToBuffer(int x, int y) {
      //Border is always wall
      if (IsCellAtBorder(x, y)) {
        _buffer[x, y] = WALL_ID;

        return;
      }

      var surroundingWalls = GetSurroundingWallCount(x, y, _buffer);

      //Use some smoothing rules to generate caves
      if (surroundingWalls > 4)
        _buffer[x, y] = WALL_ID;
      if (surroundingWalls < 4)
        _buffer[x, y] = CAVE_ID;
    }

    protected override void DisplayBuffer() => GenerateAndDisplayTexture(_buffer);
  }
}