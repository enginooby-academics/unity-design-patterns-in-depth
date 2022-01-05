using UnityEngine;
using static UnityEngine.Mathf;

namespace CompositePattern.Case2.Base1 {
  /// <summary>
  /// * A 'Leaf' class
  /// </summary>
  public class Sphere : Shape {
    public Sphere() : base(PrimitiveType.Sphere) { }

    public override double GetVolume() => 4 / 3 * PI * Pow(_scale / 2, 3);
  }
}
