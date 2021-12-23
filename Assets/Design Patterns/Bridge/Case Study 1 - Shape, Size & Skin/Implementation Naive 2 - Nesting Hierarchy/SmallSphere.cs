using UnityEngine;

namespace BridgePattern.Case1.Naive2 {
  public abstract class SmallSphere : Sphere {
    public SmallSphere() : base() => _go.SetScale(Random.Range(.5f, 1f));
  }
}
