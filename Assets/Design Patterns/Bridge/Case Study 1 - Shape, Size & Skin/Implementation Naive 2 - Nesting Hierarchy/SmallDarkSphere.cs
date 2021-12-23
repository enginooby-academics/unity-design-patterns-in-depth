using UnityEngine;

namespace BridgePattern.Case1.Naive2 {
  public class SmallDarkSphere : SmallSphere {
    public SmallDarkSphere() : base() => _go.SetMaterialColor(Color.gray);
  }
}
