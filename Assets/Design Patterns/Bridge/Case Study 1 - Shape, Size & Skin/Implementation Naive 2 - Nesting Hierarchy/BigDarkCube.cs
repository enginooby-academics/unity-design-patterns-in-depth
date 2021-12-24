using UnityEngine;

namespace BridgePattern.Case1.Naive2 {
public class BigDarkCube : BigCube {
  public BigDarkCube() : base() => _go.SetMaterialColor(Color.gray);
}
}
