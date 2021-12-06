using System;
namespace VisitorPattern.Case1.Base {
  /// <summary>
  /// * [A 'Concrete Visitor' class]
  /// Calculate the largest maximum length between two points of the shape.
  /// </summary>
  public class DiameterCalculator : Calculator {
    protected override float Calculate(ProceduralCube cube) {
      return (float)(Math.Sqrt(3) * cube.Size);
    }

    protected override float Calculate(ProceduralSphere sphere) {
      return 2 * sphere.Radius;
    }

    protected override float Calculate(ProceduralCylinder cylinder) {
      var volume = Math.PI * Math.Pow(cylinder.Radius, 2) * cylinder.Height;
      return (float)Math.Sqrt(4 * volume / cylinder.Height / Math.PI);
    }
  }
}
