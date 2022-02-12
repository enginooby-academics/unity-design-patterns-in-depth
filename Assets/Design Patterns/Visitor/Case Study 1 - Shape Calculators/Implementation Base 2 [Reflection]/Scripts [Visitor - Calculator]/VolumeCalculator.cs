using static UnityEngine.Mathf;

namespace VisitorPattern.Case1.Base2 {
  /// <summary>
  ///   * [A 'Concrete Visitor' class]
  /// </summary>
  public class VolumeCalculator : Calculator {
    protected override double Calculate(ProceduralCube cube) => Pow(cube.Size, 3);

    protected override double Calculate(ProceduralSphere sphere) => 4 / 3 * PI * Pow(sphere.Radius, 3);
  }
}