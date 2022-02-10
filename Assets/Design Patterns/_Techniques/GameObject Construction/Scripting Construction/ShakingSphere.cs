namespace GOConstruction.Scripting {
  public class ShakingSphere : Sphere {
    public ShakingSphere() : base() => _gameObject.AddComponent<Shaking>();
  }
}