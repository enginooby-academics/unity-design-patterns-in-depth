using UnityEngine;

namespace BridgePattern.Case1.Unity1 {
  /// <summary>
  /// * A 'Concrete Implementation' class
  /// </summary>
  public class Big : Size {
    void Start() => gameObject.SetScale(Random.Range(2f, 3f));
  }
}
