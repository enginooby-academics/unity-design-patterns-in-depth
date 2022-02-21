namespace GOConstruction.Scripting {
  public class RotatingCube : Cube {
    public RotatingCube() => _gameObject.AddComponent<Rotating>();
  }
}