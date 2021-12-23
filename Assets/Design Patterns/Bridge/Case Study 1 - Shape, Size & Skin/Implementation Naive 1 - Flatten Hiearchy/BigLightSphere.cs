using UnityEngine;

namespace BridgePattern.Case1.Naive {
  public class BigLightSphere : IShape {
    public void Display() {
      var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      go.SetScale(Random.Range(2f, 3f));
      go.SetMaterialColor(Color.white);
    }
  }
}
