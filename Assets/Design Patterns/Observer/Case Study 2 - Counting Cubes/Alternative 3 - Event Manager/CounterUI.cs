using UnityEngine;
using TMPro;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Alternative3 {
  [RequireComponent(typeof(TextMeshProUGUI))]
  /// <summary>
  /// * [An 'Observer' class]
  /// </summary>
  public class CounterUI : Shared.CounterUI {
    void OnEnable() => EventManager.StartListening(FindObjectOfType<Counter>().OnCountUpEvent, SetText);
  }
}
