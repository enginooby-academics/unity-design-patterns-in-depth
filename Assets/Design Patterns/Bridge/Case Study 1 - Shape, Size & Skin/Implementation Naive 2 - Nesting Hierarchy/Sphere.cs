using UnityEngine;
using static UnityEngine.GameObject;

namespace BridgePattern.Case1.Naive2 {
  public abstract class Sphere : Shape {
    protected Sphere() => _go = CreatePrimitive(PrimitiveType.Cube);
  }
}
