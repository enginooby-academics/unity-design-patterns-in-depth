using UnityEngine;
using TMPro;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Base1 {
  [RequireComponent(typeof(TextMeshProUGUI))]
  /// <summary>
  /// * [A Concrete 'Observer' class]
  /// </summary>
  public class CounterUI : Shared.CounterUI, ICountObserver {
    public void OnCountChanged(int count) => SetText(count);
  }
}
