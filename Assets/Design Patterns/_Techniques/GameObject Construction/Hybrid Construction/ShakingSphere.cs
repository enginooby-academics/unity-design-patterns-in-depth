namespace GOConstruction.Hybrid {
  public class ShakingSphere : Sphere {
    protected override void Awake() {
      base.Awake();
      gameObject.AddComponent<Shaking>();
    }
  }
}