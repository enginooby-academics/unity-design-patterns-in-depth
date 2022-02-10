namespace BridgePattern.Case1.Base.WithAbstractFactory.Base {
  public class SmallSphereFactory : SphereFactory {
    public override ISize CreateSize() => new Small();
  }
}