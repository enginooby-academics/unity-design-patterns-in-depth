using UnityEngine;
using TMPro;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Alternative1 {
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class CounterUI : Shared.CounterUI {
    [SerializeField]
    private ReferenceIntSO _countRef;

    // ! Costly-performance for tracking SO state, esp. with expensive Action
    // void Update() => SetText(_countVar.Value);

    // ! Improve performance
    void Update() => _countRef.PerformOnValueChanged(SetText);
  }
}
