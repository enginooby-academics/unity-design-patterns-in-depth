using System;
using UnityEngine;

namespace VisitorPattern.Case1.Base {
  /// <summary>
  /// * [A 'Concrete Visitor' class]
  /// </summary>
  public class SurfaceAreaCalculator : Calculator {
    protected override float Calculate(ProceduralCube cube) {
      return (float)(6 * Math.Pow(cube.Size, 2));
    }

    protected override float Calculate(ProceduralSphere sphere) {
      return 4 * Mathf.PI * Mathf.Pow(sphere.Radius, 2);
    }
  }
}
