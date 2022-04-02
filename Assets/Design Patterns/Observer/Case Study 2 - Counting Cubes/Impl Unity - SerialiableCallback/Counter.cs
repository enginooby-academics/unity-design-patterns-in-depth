#if ASSET_SERIALIZABLE_CALLBACK
using System;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.ImplSerializableCallback {
  /// <summary>
  ///   * The 'Subject' class
  /// </summary>
  public class Counter : Shared.Counter {
    // ! 1. Declare event in the Subject 
    public IntEvent OnCountUpEvent;

    public override int Count {
      set {
        _count = value;
        // ! 2. Invoke the event
        OnCountUpEvent?.Invoke(_count);
      }
    }
  }

  // ! 0. Create serializable class
  [Serializable]
  public class IntEvent : SerializableEvent<int> { }
}
#endif