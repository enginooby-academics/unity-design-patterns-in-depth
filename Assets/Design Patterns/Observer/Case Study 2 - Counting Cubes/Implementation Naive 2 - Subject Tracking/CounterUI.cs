using UnityEngine;
using TMPro;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Naive2 {
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class CounterUI : Shared.CounterUI {
    // ! Tight coupling with the subject
    // ! Costly-performance for tracking subject's state
    void Update() => SetText(Counter.Instance.Count);
  }
}
