using UnityEngine;

namespace BridgePattern.Case1.Base {
  public class ShapeSquare : Shape {
    public ShapeSquare() : base() { }

    public ShapeSquare(IColor shapeColor, IDimension shapeDimension) : base(shapeColor, shapeDimension) {
    }

    protected override GameObject MakeShape() {
      return GameObject.CreatePrimitive(PrimitiveType.Cube);
    }
  }
}
