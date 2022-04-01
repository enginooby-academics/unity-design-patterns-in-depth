using TMPro;
using UnityEngine;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Base1 {
  /// <summary>
  /// * [A Concrete 'Observer' class]
  /// </summary>
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class CounterUI : Shared.CounterUI, ICountObserver {
    public void OnCountChanged(int count) => SetText(count);
  }
}