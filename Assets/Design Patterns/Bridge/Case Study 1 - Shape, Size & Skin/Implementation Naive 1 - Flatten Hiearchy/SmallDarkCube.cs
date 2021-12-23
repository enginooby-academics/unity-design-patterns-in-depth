using UnityEngine;

namespace BridgePattern.Case1.Naive {
  public class SmallDarkCube : IShape {
    public void Display() {
      var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
      go.SetScale(Random.Range(.5f, 1f));
      go.SetMaterialColor(Color.gray);
    }
  }
}
