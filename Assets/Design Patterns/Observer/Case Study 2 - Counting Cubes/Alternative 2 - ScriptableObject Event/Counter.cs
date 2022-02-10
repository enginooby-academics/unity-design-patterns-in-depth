using UnityEngine;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Alternative2 {
  public class Counter : Shared.Counter {
    [SerializeField]
    private IntEventSO _onCountUpEvent;

    public override int Count {
      set {
        _count = value;
        _onCountUpEvent.NotifyObservers(_count);
      }
    }
  }
}
