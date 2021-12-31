using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern.Case2.Alternative2 {
  public class OneVarEventSO<T> : ScriptableObject {
    private List<OneVarObserver<T>> observers = new List<OneVarObserver<T>>();

    public void NotifyObservers(T eventValue) {
      for (int i = observers.Count - 1; i >= 0; i--)
        observers[i].OnNotified(eventValue);
    }

    public void AddListener(OneVarObserver<T> observer) => observers.Add(observer);

    public void RemoveListener(OneVarObserver<T> listener) => observers.Remove(listener);
  }
}