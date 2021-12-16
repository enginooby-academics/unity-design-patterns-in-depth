using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BridgePattern.Case1.Base {
  public class ShapeCircle : Shape {
    public ShapeCircle() : base() { }

    public ShapeCircle(IColor shapeColor, IDimension shapeDimension) : base(shapeColor, shapeDimension) {
    }

    protected override GameObject MakeShape() {
      return GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }
  }
}
