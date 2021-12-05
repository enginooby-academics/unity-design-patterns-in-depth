using UnityEngine;

namespace MediatorPattern.Case1.Naive {
  public class Sphere : Shape {
    private void Reset() {
      gameObject.name = "Sphere";
      gameObject.SetPrimitiveMesh(PrimitiveType.Sphere);
    }

    private void Start() {
      // collide with different shapes
      _collidableShapes.Add(typeof(Cube));
      _collidableShapes.Add(typeof(Cylinder));
      // collide with same shape
      // _collidableShapes.Add(typeof(Sphere));
    }
  }
}
