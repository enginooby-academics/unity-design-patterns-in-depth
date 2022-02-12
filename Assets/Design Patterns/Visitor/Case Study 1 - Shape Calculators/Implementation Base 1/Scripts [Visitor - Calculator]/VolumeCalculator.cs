using static UnityEngine.Mathf;

namespace VisitorPattern.Case1.Base1 {
  /// <summary>
  ///   * [A 'Concrete Visitor' class]
  /// </summary>
  public class VolumeCalculator : Calculator {
    public override double Calculate(ProceduralCube cube) => Pow(cube.Size, 3);

    public override double Calculate(ProceduralSphere sphere) => 4 / 3 * PI * Pow(sphere.Radius, 3);
  }
}