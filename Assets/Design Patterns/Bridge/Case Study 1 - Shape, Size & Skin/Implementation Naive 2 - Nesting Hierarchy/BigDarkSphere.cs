using UnityEngine;

namespace BridgePattern.Case1.Naive2 {
  public class BigDarkSphere : BigSphere {
    public BigDarkSphere() : base() => _go.SetMaterialColor(Color.gray);
  }
}
