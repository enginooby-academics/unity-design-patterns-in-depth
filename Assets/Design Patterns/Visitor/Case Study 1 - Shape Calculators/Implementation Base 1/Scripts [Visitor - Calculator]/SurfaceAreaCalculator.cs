using static UnityEngine.Mathf;

namespace VisitorPattern.Case1.Base1 {
  /// <summary>
  ///   * [A 'Concrete Visitor' class]
  /// </summary>
  public class SurfaceAreaCalculator : Calculator {
    public override double Calculate(ProceduralCube cube) => 6 * Pow(cube.Size, 2);

    public override double Calculate(ProceduralSphere sphere) => 4 * PI * Pow(sphere.Radius, 2);
  }
}