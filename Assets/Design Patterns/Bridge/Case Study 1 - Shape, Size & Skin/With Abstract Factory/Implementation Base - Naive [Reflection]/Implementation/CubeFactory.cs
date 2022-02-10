namespace BridgePattern.Case1.Base.WithAbstractFactory.Base {
  public abstract class CubeFactory : ShapeFactory {
    public override Shape CreateShape() => new Cube();
    public override ISize CreateSize() => new Small();
  }
}