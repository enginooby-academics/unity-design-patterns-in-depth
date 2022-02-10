namespace GOConstruction.Scripting {
  public class RotatingSphere : Sphere {
    public RotatingSphere() : base() => _gameObject.AddComponent<Rotating>();
  }
}
