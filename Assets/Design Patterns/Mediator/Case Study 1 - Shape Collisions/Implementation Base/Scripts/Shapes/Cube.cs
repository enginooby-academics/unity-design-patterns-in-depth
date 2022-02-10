using UnityEngine;

namespace MediatorPattern.Case1.Base {
  /// <summary>
  /// * A 'Concrete Colleague' class
  /// </summary>
  public class Cube : Shape {
    private void Reset() {
      gameObject.name = "Cube";
      gameObject.SetPrimitiveMesh(PrimitiveType.Cube);
    }
  }
}
