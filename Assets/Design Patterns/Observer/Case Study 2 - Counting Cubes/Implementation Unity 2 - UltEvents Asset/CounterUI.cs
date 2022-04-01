#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif
using TMPro;
using UltEvents;
using UnityEngine;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Unity2 {
  /// <summary>
  /// * [An 'Observer' class]
  /// </summary>
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class CounterUI : Shared.CounterUI {
    private void OnEnable() => FindObjectOfType<Counter>().OnCountUpEvent.DynamicCalls += SetText;

    private void OnDisable() => FindObjectOfType<Counter>().OnCountUpEvent.DynamicCalls -= SetText;

    // ! Same as UnityEvent
    [Button]
    public void ExploitEvent() => FindObjectOfType<Counter>().OnCountUpEvent.Invoke(100);
  }
}