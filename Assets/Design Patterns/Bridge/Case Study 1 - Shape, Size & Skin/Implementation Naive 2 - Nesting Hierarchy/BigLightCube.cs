using UnityEngine;

namespace BridgePattern.Case1.Naive2 {
  public class BigLightCube : BigCube {
    public BigLightCube() : base() => _go.SetMaterialColor(Color.white);
  }
}
