#if ASSET_SERIALIZABLE_CALLBACK
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif
using TMPro;
using UnityEngine;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.ImplSerializableCallback {
  /// <summary>
  /// * An 'Observer' class
  /// </summary>
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class CounterUI : Shared.CounterUI {
    private void Start() {
      // ? How to subscribe listener in script
      // (FindObjectOfType<Counter>().OnCountUpEvent.invokable as InvokableEvent<int>).action += SetText;
    }
  }
}
#endif