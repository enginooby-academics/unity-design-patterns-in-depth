namespace GOConstruction.Scripting {
  public class RotatingCube : Cube {
    public RotatingCube() : base() => _gameObject.AddComponent<Rotating>();
  }
}
