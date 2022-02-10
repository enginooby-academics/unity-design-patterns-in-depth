using UnityEngine;
using TMPro;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.CSharp {
  [RequireComponent(typeof(TextMeshProUGUI))]
  /// <summary>
  /// * [An 'Observer' class]
  /// </summary>
  public class CounterUI : Shared.CounterUI {
    // ! 1. With Action (secured)
    private void OnEnable() => Counter.OnCountUpEvent += SetText;
    private void OnDisable() => Counter.OnCountUpEvent -= SetText;

    // ! 2. With Delegate/Func and return type (not secured, can be set to null)
    // private void OnEnable() => Counter.OnCountUpEvent += SetTextAndReturnWord;
    // private void OnDisable() => Counter.OnCountUpEvent -= SetTextAndReturnWord;

    // public string SetTextAndReturnWord(int count) {
    //   SetText(count);
    //   return count.ToWord();
    // }
  }
}
