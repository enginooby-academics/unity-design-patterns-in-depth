using UnityEngine.Events;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Unity1 {
  /// <summary>
  /// * [The 'Subject' class]
  /// </summary>
  public class Counter : Shared.Counter {
    // ! 1. Declare UnityEvent in the Subject 
    // ! (if use static for convenient ref, will not show in the inspector)
    public UnityEvent<int> OnCountUpEvent = new UnityEvent<int>();

    public override int Count {
      set {
        _count = value;
        // ! 2. Invoke the event
        OnCountUpEvent?.Invoke(_count);
      }
    }
  }
}
