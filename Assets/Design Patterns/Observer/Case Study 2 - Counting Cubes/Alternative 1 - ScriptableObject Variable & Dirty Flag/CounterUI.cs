using TMPro;
using UnityEngine;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Alternative1 {
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class CounterUI : Shared.CounterUI {
    [SerializeField] private ReferenceIntSO _countRef;

    // ! Alternative: get countRef from the Counter w/o SerializeField (coupling)
    // void Start() => _countRef = FindObjectOfType<Counter>().CountRef;

    // ! Costly-performance for tracking SO state, esp. with expensive Action
    // void Update() => SetText(_countVar.Value);

    // ! Improve performance
    private void Update() => _countRef.PerformOnValueChanged(SetText);
  }
}