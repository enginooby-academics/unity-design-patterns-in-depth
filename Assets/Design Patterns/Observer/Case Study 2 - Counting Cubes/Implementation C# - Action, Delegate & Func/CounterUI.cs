using System;
using TMPro;
using UnityEngine;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.CSharp {
  /// <summary>
  /// * [An 'Observer' class]
  /// </summary>
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class CounterUI : Shared.CounterUI {
    private void OnEnable() {
      Counter.OnCountUpDelegate += SetTextAndReturnWord;
      Counter.OnCountUpFunc += SetTextAndReturnWord;
      Counter.OnCountUpAction += SetText;
    }

    private void OnDisable() {
      Counter.OnCountUpDelegate -= SetTextAndReturnWord;
      Counter.OnCountUpFunc -= SetTextAndReturnWord;
      Counter.OnCountUpAction -= SetText;
    }

    private string SetTextAndReturnWord(int count) {
      SetText(count);
      return count.ToWord();
    }
  }
}