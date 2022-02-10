namespace BridgePattern.Case1.Base.WithAbstractFactory.Base {
  public abstract class SphereFactory : ShapeFactory {
    public override Shape CreateShape() => new Sphere();
    public override ISkin CreateSkin() => new Light();
  }
}