using UnityEngine;

namespace VisitorPattern.Case1.Base {
  /// <summary>
  /// * [A 'Concrete Visitor' class]
  /// </summary>
  public class VolumeCalculator : Calculator {
    protected override float Calculate(ProceduralCube cube) {
      return Mathf.Pow(cube.Size, 3);
    }

    protected override float Calculate(ProceduralSphere sphere) {
      return 4 / 3 * Mathf.PI * Mathf.Pow(sphere.Radius, 3);
    }
  }
}
