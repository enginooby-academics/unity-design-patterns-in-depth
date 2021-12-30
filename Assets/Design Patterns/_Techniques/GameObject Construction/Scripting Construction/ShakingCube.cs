namespace GOConstruction.Scripting {
  public class ShakingCube : Cube {
    public ShakingCube() : base() => _gameObject.AddComponent<Shaking>();
  }
}