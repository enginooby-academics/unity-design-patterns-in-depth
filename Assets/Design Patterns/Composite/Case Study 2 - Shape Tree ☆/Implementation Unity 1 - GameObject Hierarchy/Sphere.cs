using UnityEngine;
using static UnityEngine.Mathf;

namespace CompositePattern.Case2.Unity1 {
  /// <summary>
  /// * A 'Leaf' class
  /// </summary>
  public class Sphere : Shape {
    protected override void Start() {
      base.Start();
      gameObject.name = "Sphere";
      gameObject.SetPrimitiveMesh(PrimitiveType.Sphere);
    }

    public override double GetVolume() => 4 / 3 * PI * Pow(_scale / 2, 3);
  }
}
