using System;
using Object = UnityEngine.Object;

namespace ObserverPattern.Case2.Base1 {
  [Serializable]
  public class ICountObserverContainer : IUnifiedContainer<ICountObserver> {
    public ICountObserverContainer(ICountObserver observer) => ObjectField = observer as Object;
  }
}