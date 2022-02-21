namespace GOConstruction.Hybrid {
  public class ShakingCube : Cube {
    protected override void Awake() {
      base.Awake();
      gameObject.AddComponent<Shaking>();
    }
  }
}