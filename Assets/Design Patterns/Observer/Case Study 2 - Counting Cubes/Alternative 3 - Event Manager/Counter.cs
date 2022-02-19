using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Alternative3 {
  /// <summary>
  ///   * [The 'Subject' class]
  /// </summary>
  public class Counter : Shared.Counter {
    public Event<int> OnCountUpEvent = new Event<int>();

    public override int Count {
      set {
        _count = value;
        EventManager.NotifyEvent(OnCountUpEvent, _count);
      }
    }
  }
}