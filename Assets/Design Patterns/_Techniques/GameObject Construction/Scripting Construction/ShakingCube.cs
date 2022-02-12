namespace GOConstruction.Scripting {
  public class ShakingCube : Cube {
    public ShakingCube() => _gameObject.AddComponent<Shaking>();
  }
}