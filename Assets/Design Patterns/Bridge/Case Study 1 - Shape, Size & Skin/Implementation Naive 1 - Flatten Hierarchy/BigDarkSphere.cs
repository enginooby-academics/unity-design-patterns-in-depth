using UnityEngine;

namespace BridgePattern.Case1.Naive {
  public class BigDarkSphere : IShape {
    public void Display() {
      var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      go.SetScale(Random.Range(2f, 3f));
      go.SetMaterialColor(Color.gray);
    }
  }
}
