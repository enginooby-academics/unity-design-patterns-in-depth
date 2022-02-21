namespace GOConstruction.Scripting {
  public class RotatingSphere : Sphere {
    public RotatingSphere() => _gameObject.AddComponent<Rotating>();
  }
}