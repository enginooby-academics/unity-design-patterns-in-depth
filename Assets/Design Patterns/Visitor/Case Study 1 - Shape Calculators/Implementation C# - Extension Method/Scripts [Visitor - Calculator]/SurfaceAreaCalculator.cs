using static UnityEngine.Mathf;

namespace VisitorPattern.Case1.CSharp {
  /// <summary>
  ///   * [A 'Concrete Visitor' class]
  /// </summary>
  public class SurfaceAreaCalculator : Calculator {
    protected override double Calculate(ProceduralCube cube) => 6 * Pow(cube.Size, 2);

    protected override double Calculate(ProceduralSphere sphere) => 4 * PI * Pow(sphere.Radius, 2);
  }
}