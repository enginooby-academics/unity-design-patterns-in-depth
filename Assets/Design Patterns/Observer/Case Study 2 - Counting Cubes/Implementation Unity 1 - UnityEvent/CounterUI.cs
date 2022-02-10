using UnityEngine;
using TMPro;
using Shared = ObserverPattern.Case2;
using Sirenix.OdinInspector;

namespace ObserverPattern.Case2.Unity1 {
  [RequireComponent(typeof(TextMeshProUGUI))]
  /// <summary>
  /// * [An 'Observer' class]
  /// </summary>
  public class CounterUI : Shared.CounterUI {
    // ! 3. Register subject's event from the observer (scripting binding)
    // Alternative: serialize in the inspector (UI binding)
    // Cannot access OnCountUpEvent via singleton Instance which returns super singleton class
    // ! Still couple with the subject but ok in this case
    // void Start() => FindObjectOfType<Counter>().OnCountUpEvent.AddListener(SetText);
    void Start() => FindObjectOfType<Counter>().ListenOnCountUpEvent(SetText);

    [Button]
    public void ExploitUnityEvent() {
      // ! Cons: poor security - observer can modify or invoke subject's UnityEvent
      // ! Solution: private UnityEvent and expose only AddListener() 
      FindObjectOfType<Counter>().OnCountUpEvent.RemoveAllListeners();
      FindObjectOfType<Counter>().OnCountUpEvent.Invoke(100);
    }
  }
}
