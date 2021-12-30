using UnityEngine;
using TMPro;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Unity1 {
  [RequireComponent(typeof(TextMeshProUGUI))]
  /// <summary>
  /// * [An 'Observer' class]
  /// </summary>
  public class CounterUI : Shared.CounterUI {
    // ! 3. Register subject's event from the observer (scripting binding)
    // Alternative: serialize in the inspector (UI binding)
    // Cannot access OnCountUpEvent via singleton Instance which returns super singleton class
    void Start() => FindObjectOfType<Counter>().OnCountUpEvent.AddListener(SetText);
  }
}
