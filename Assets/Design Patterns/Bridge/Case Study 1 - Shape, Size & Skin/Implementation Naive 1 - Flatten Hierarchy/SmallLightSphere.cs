using UnityEngine;

namespace BridgePattern.Case1.Naive {
  public class SmallLightSphere : IShape {
    public void Display() {
      var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      go.SetScale(Random.Range(.5f, 1f));
      go.SetMaterialColor(Color.white);
    }
  }
}
