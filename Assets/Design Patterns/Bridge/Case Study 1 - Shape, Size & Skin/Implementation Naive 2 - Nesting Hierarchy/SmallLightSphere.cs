using UnityEngine;

namespace BridgePattern.Case1.Naive2 {
  public class SmallLightSphere : SmallSphere {
    public SmallLightSphere() : base() => _go.SetMaterialColor(Color.white);
  }
}
