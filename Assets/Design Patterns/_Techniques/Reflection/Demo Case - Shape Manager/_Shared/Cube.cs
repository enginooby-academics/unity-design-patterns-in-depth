using UnityEngine;

namespace Reflection.Case1 {
  public class Cube : IShape {
    public Cube() => GameObject.CreatePrimitive(PrimitiveType.Cube);
    public float GetVolume() => Mathf.Pow(1, 3);
  }
}
