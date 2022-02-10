using System.Diagnostics;
using UnityEngine;

namespace BridgePattern.Case1.Naive {
  public class BigDarkCube : IShape {
    public void Display() {
      var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
      go.SetScale(Random.Range(2f, 3f));
      go.SetMaterialColor(Color.gray);
    }
  }
}
