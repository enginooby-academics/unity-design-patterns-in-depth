using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BridgePattern.Case1.Base {
  /// <summary>
  /// Abstraction.
  /// </summary>
  // ? Convert to abstract class
  public abstract class Shape {
    protected IColor _shapeColor;
    protected IDimension _shapeDimension;

    public Shape() { }

    public Shape(IColor shapeColor, IDimension shapeDimension) {
      _shapeColor = shapeColor;
      _shapeDimension = shapeDimension;
    }

    public void SetColor(IColor shapeColor) {
      _shapeColor = shapeColor;
    }

    public void SetDimension(IDimension shapeDimension) {
      _shapeDimension = shapeDimension;
    }

    public void Draw() {
      GameObject shape = MakeShape();
      _shapeColor.MakeColor(shape);
      _shapeDimension.MakeDimension(shape);
    }

    protected abstract GameObject MakeShape();
  }
}
