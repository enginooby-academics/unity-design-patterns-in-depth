namespace BridgePattern.Case1.Base.WithAbstractFactory.Base {
  public class DarkCubeFactory : CubeFactory {
    public override ISkin CreateSkin() => new Dark();
  }
}