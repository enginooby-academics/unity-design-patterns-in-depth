using UnityEngine;

namespace BridgePattern.Case1.Unity1 {
  /// <summary>
  /// * [A 'Concrete Implementation' class]
  /// </summary>
  public class Small : Size {
    void Start() => gameObject.SetScale(Random.Range(.5f, 1f));
  }
}
