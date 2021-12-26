using UnityEngine;

namespace BridgePattern.Case1.Base {
  /// <summary>
  /// * [A 'Concrete Implementation' class]
  /// </summary>
  public class Big : ISize {
    public void ProcessSize(GameObject go) => go.SetScale(Random.Range(2f, 3f));
  }
}
