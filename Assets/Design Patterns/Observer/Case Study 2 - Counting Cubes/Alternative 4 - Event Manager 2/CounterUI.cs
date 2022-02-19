using TMPro;
using UnityEngine;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Alternative4 {
  /// <summary>
  /// * [An 'Observer' class]
  /// </summary>
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class CounterUI : Shared.CounterUI, IObserver {
    private void OnEnable() => EventManager.Instance.AddObserver(CounterEvent.OnCountUp, this, SetText);

    private void SetText(EventHandlerParam param) => SetText((int) param.Param);
  }
}