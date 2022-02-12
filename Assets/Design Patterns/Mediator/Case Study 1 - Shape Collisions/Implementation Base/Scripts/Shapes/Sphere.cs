using UnityEngine;

namespace MediatorPattern.Case1.Base {
  /// <summary>
  ///   * A 'Concrete Colleague' class
  /// </summary>
  public class Sphere : Shape {
    private void Reset() {
      gameObject.name = "Sphere";
      gameObject.SetPrimitiveMesh(PrimitiveType.Sphere);
    }
  }
}