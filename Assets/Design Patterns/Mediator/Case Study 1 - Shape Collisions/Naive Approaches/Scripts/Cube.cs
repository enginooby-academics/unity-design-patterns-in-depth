using UnityEngine;

namespace MediatorPattern.Case1.Naive {
  public class Cube : Shape {
    private void Reset() {
      gameObject.name = "Cube";
      gameObject.SetPrimitiveMesh(PrimitiveType.Cube);
    }

    private void Start() {
      // ! In naive approach, the other colliguages are referenced hardcodely,
      // ! make them tightly coupled
      // collide with different shapes
      _collidableShapes.Add(typeof(Cylinder));
      _collidableShapes.Add(typeof(Sphere));
      // collide with same shape
      // _collidableShapes.Add(typeof(Cube));
    }
  }
}