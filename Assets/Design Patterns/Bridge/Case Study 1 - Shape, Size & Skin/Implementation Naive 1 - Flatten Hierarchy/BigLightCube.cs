using UnityEngine;

namespace BridgePattern.Case1.Naive {
  public class BigLightCube : IShape {
    public void Display() {
      var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
      go.SetScale(Random.Range(2f, 3f));
      go.SetMaterialColor(Color.white);
    }
  }
}
