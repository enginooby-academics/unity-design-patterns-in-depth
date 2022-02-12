using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Naive1 {
  public class Counter : Shared.Counter {
    private CounterUI _counterUI;

    public override int Count {
      set {
        _count = value;
        // ! Tight coupling
        _counterUI.SetText(_count);
      }
    }

    private void Start() => _counterUI = FindObjectOfType<CounterUI>();
  }
}