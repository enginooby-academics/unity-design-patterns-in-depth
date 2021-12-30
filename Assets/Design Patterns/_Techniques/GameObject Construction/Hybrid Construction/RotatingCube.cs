namespace GOConstruction.Hybrid {
  public class RotatingCube : Cube {
    protected override void Awake() {
      base.Awake();
      gameObject.AddComponent<Rotating>();
    }
  }
}
