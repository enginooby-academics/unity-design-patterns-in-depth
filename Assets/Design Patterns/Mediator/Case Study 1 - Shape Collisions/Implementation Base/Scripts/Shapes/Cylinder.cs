using UnityEngine;

namespace MediatorPattern.Case1.Base {
  /// <summary>
  ///   * A 'Concrete Colleague' class
  /// </summary>
  public class Cylinder : Shape {
    private void Reset() {
      gameObject.name = "Cylinder";
      gameObject.SetPrimitiveMesh(PrimitiveType.Cylinder);
    }
  }
}