using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern.Case2.Alternative2 {
  public class MonoParamEventSO<T> : ScriptableObject {
    private readonly List<MonoParamObserver<T>> _observers = new();

    public void NotifyObservers(T eventValue) {
      for (var i = _observers.Count - 1; i >= 0; i--)
        _observers[i].OnNotified(eventValue);
    }

    public void AddObserver(MonoParamObserver<T> observer) => _observers.Add(observer);

    public void RemoveObserver(MonoParamObserver<T> observer) => _observers.Remove(observer);
  }
} 