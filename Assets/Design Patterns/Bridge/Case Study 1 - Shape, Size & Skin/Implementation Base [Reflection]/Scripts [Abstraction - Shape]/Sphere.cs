using UnityEngine;
using static UnityEngine.GameObject;

namespace BridgePattern.Case1.Base {
  /// <summary>
  /// * [The 'Refined Abstraction' class]
  /// </summary>
  public class Sphere : Shape {
    protected override GameObject CreateGameObject() => CreatePrimitive(PrimitiveType.Sphere);
  }
}
