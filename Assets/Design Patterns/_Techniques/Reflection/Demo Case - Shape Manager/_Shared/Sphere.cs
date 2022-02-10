using UnityEngine;

namespace Reflection.Case1 {
  public class Sphere : IShape {
    public Sphere() => GameObject.CreatePrimitive(PrimitiveType.Sphere);
    public float GetVolume() => 4 / 3 * Mathf.PI * Mathf.Pow(1, 3);
  }
}
