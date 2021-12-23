using UnityEngine;

namespace BridgePattern.Case1.Base {
  /// <summary>
  /// * [A 'Concrete Implementation' class]
  /// </summary>
  public class Small : ISize {
    public void ProcessSize(GameObject go) => go.SetScale(Random.Range(.5f, 1f));
  }
}
