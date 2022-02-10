namespace GOConstruction.Hybrid {
  public class RotatingSphere : Sphere {
    protected override void Awake() {
      base.Awake();
      gameObject.AddComponent<Rotating>();
    }
  }
}
