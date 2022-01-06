using UnityEngine;
using static UnityEngine.Mathf;

namespace CompositePattern.Case2.Unity1 {
  /// <summary>
  /// * A 'Leaf' class
  /// </summary>
  public class Cube : Shape {
    protected override void Start() {
      base.Start();
      gameObject.name = "Cube";
      gameObject.SetPrimitiveMesh(PrimitiveType.Cube);
    }

    public override double GetVolume() => Pow(_scale, 3);
  }
}
