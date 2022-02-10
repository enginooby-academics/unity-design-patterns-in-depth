using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Naive1 {
  public class Counter : Shared.Counter {
    public override int Count {
      set {
        _count = value;
        // ! Tight coupling
        _counterUI.SetText(_count);
      }
    }

    private CounterUI _counterUI;
    private void Start() => _counterUI = FindObjectOfType<CounterUI>();
  }
}
