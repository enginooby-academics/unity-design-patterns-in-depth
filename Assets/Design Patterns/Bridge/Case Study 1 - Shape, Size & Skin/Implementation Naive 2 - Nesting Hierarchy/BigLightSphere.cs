using UnityEngine;

namespace BridgePattern.Case1.Naive2 {
  public class BigLightSphere : BigSphere {
    public BigLightSphere() : base() => _go.SetMaterialColor(Color.white);
  }
}
