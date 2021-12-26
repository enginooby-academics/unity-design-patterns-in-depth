namespace BridgePattern.Case1.Base.WithAbstractFactory.Base {
  public class BigSphereFactory : SphereFactory {
    public override ISize CreateSize() => new Big();
  }
}