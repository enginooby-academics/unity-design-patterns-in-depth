using TMPro;
using UnityEngine;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Alternative3 {
  /// <summary>
  /// * [An 'Observer' class]
  /// </summary>
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class CounterUI : Shared.CounterUI {
    private void OnEnable() =>
      EventManager.Instance.AddEventHandler(FindObjectOfType<Counter>().OnCountUpEvent, SetText);
  }
}