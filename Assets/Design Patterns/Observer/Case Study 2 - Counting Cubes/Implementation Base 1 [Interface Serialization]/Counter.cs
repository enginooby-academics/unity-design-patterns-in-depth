using System.Collections.Generic;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Base1 {
  /// <summary>
  /// * [The 'Subject' class]
  /// </summary>
  public class Counter : Shared.Counter {
    // Serialized interface for UI workflow instead of Add/RemoveSubcribers()
    [UnityEngine.SerializeField]
    private List<ICountObserverContainer> _countObservers = new List<ICountObserverContainer>();

    // TODO: Implement Add/RemoveSubcribers() for ICountObserverContainer/ICountObseerver

    public void NotifyCountObservers() {
      _countObservers.ForEach(observer => observer.Result.OnCountChanged(Count));
    }

    public override int Count {
      set {
        _count = value;
        NotifyCountObservers();
      }
    }
  }
}
