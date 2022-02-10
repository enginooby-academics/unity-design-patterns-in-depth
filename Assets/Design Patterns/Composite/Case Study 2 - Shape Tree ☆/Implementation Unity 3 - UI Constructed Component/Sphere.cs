using static UnityEngine.Mathf;

namespace CompositePattern.Case2.Unity3 {
  /// <summary>
  /// * A 'Leaf' class
  /// </summary>
  public class Sphere : Shape {
    public override double GetVolume() => 4 / 3 * PI * Pow(_scale / 2, 3);
  }
}
