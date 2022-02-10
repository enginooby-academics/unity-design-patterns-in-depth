using UnityEngine;

namespace BridgePattern.Case1.Unity2 {
  /// <summary>
  /// * [A 'Concrete Implementation' class]
  /// </summary>
  public class Light : Skin {
    void Start() => gameObject.SetMaterialColor(Color.white);
  }
}
