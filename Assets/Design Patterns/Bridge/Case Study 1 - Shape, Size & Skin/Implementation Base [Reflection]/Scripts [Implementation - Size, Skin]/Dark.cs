using UnityEngine;

namespace BridgePattern.Case1.Base {
  /// <summary>
  /// * [A 'Concrete Implementation' class]
  /// </summary>
  public class Dark : ISkin {
    public void ProcessSkin(GameObject go) => go.SetMaterialColor(Color.gray);
  }
}
