namespace BridgePattern.Case1.Base.WithAbstractFactory.Base {
  public abstract class ShapeFactory {
    public Shape GetShape() {
      Shape shape = CreateShape();
      shape.SetSize(CreateSize());
      shape.SetSkin(CreateSkin());
      shape.Display();
      return shape;
    }

    public abstract Shape CreateShape();
    public abstract ISize CreateSize();
    public abstract ISkin CreateSkin();
  }
}