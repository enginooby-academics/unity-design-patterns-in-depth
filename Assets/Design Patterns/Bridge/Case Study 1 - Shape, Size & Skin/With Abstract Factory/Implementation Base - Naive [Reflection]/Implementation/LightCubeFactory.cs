namespace BridgePattern.Case1.Base.WithAbstractFactory.Base {
  public class LightCubeFactory : CubeFactory {
    public override ISkin CreateSkin() => new Light();
  }
}