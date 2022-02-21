namespace GOConstruction.Scripting {
  public class ShakingSphere : Sphere {
    public ShakingSphere() => _gameObject.AddComponent<Shaking>();
  }
}