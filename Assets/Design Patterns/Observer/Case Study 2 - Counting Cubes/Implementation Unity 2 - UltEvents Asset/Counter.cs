using UltEvents;
using UnityEngine.Events;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Unity2 {
  /// <summary>
  /// * [The 'Subject' class]
  /// </summary>
  public class Counter : Shared.Counter {
    // ! Process similar to UnityEvent
    [UnityEngine.SerializeField]
    public UltEvent<int> OnCountUpEvent = new UltEvent<int>();

    // Secure UnityEvent while expose AddListener()
    public void ListenOnCountUpEvent(UnityAction<int> action) {
      // OnCountUpEvent.AddListener(action);
    }

    public override int Count {
      set {
        _count = value;
        OnCountUpEvent?.Invoke(_count);
      }
    }
  }
}
