using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Alternative4 {
  /// <summary>
  ///   * [The 'Subject' class]
  /// </summary>
  public class Counter : Shared.Counter {
    public override int Count {
      set {
        _count = value;
        EventManager.Instance.NotifyEvent(CounterEvent.OnCountUp, _count);
      }
    }
  }

  public enum CounterEvent {
    OnCountUp,
  }
}