using UnityEngine;

namespace BridgePattern.Case1.Naive2 {
  public abstract class BigSphere : Sphere {
    public BigSphere() : base() => _go.SetScale(Random.Range(2f, 3f));
  }
}
