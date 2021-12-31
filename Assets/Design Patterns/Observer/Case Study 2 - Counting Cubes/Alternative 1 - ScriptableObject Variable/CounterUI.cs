using UnityEngine;
using TMPro;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Alternative1 {
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class CounterUI : Shared.CounterUI {
    [SerializeField]
    private IntVariable _countVar;

    // ! Costly-performance for tracking SO state
    void Update() => SetText(_countVar.Value);
  }
}
