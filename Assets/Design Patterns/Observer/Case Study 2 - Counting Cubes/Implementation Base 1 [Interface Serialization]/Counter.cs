using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Shared = ObserverPattern.Case2;

namespace ObserverPattern.Case2.Base1 {
  /// <summary>
  ///   * [The 'Subject' class]
  /// </summary>
  public class Counter : Shared.Counter {
    [SerializeField] private List<ICountObserverContainer> _countObservers = new();

    public override int Count {
      set {
        _count = value;
        NotifyCountObservers();
      }
    }

    public void AddCountObserver(ICountObserver observer) {
      _countObservers.Add(new ICountObserverContainer(observer));
    }

    public void RemoveCountObserver(ICountObserver observer) {
      var observerContainer = _countObservers.FirstOrDefault(o => o.Result == observer);
      _countObservers.Remove(observerContainer);
    }

    public void NotifyCountObservers() {
      _countObservers.ForEach(observer => observer.Result.OnCountChanged(Count));
    }
  }
}