#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif
using TMPro;
using UltEvents;
using UnityEngine;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Unity2 {
  [RequireComponent(typeof(TextMeshProUGUI))]
  /// <summary>
  /// * [An 'Observer' class]
  /// </summary>
  public class CounterUI : Shared.CounterUI {
    private UltEvent<int> _onCountUp;
    private UltEvent<int> OnCountUp => _onCountUp ??= FindObjectOfType<Counter>().OnCountUpEvent;

    private void OnEnable() => OnCountUp.DynamicCalls += SetText;

    private void OnDisable() => OnCountUp.DynamicCalls -= SetText;

    [Button]
    public void ExploitEvent() {
      // ! Same as UnityEvent
      _onCountUp.Invoke(100);
    }
  }
}