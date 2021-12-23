using UnityEngine;

namespace BridgePattern.Case1.Naive2 {
  public abstract class SmallCube : Cube {
    public SmallCube() : base() => _go.SetScale(Random.Range(.5f, 1f));
  }
}
