using UnityEngine;
using static UnityEngine.GameObject;

namespace BridgePattern.Case1.Naive2 {
public abstract class Cube : Shape {
  protected Cube() => _go = CreatePrimitive(PrimitiveType.Cube);
}
}
