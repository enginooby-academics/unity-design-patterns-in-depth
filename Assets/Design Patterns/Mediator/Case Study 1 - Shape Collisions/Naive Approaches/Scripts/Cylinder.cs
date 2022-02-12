using UnityEngine;

namespace MediatorPattern.Case1.Naive {
  public class Cylinder : Shape {
    private void Reset() {
      gameObject.name = "Cylinder";
      gameObject.SetPrimitiveMesh(PrimitiveType.Cylinder);
    }

    private void Start() {
      // collide with different shapes
      _collidableShapes.Add(typeof(Cube));
      _collidableShapes.Add(typeof(Sphere));
      // collide with same shape
      // _collidableShapes.Add(typeof(Cylinder));
    }
  }
}