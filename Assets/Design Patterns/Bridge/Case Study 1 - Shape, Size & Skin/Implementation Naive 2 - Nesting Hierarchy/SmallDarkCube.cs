using UnityEngine;

namespace BridgePattern.Case1.Naive2 {
  public class SmallDarkCube : SmallCube {
    public SmallDarkCube() : base() => _go.SetMaterialColor(Color.gray);
  }
}
