using UnityEngine;

namespace BridgePattern.Case1.Unity1 {
  /// <summary>
  /// * [A 'Concrete Implementation' class]
  /// </summary>
  public class Dark : Skin {
    void Start() => gameObject.SetMaterialColor(Color.gray);
  }
}
