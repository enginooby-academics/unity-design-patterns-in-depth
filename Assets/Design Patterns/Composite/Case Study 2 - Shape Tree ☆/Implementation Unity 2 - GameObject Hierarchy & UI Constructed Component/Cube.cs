using static UnityEngine.Mathf;

namespace CompositePattern.Case2.Unity2 {
  /// <summary>
  /// * A 'Leaf' class
  /// </summary>
  public class Cube : Shape {
    public override double GetVolume() => Pow(_scale, 3);
  }
}
