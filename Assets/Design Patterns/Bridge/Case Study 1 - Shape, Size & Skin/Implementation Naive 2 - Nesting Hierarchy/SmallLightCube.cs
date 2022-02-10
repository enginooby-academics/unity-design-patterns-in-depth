using UnityEngine;

namespace BridgePattern.Case1.Naive2 {
  public class SmallLightCube : SmallCube {
    public SmallLightCube() : base() => _go.SetMaterialColor(Color.white);
  }
}
