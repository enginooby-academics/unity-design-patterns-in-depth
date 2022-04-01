using UnityEngine;
using UnityEngine.Events;

namespace ObserverPattern.Case2.Alternative2 {
  public class MonoParamObserver<T> : MonoBehaviour {
    public MonoParamEventSO<T> Event;

    public UnityEvent<T> Response;

    private void OnEnable() => Event.AddObserver(this);

    private void OnDisable() => Event.RemoveObserver(this);

    public void OnNotified(T eventValue) => Response.Invoke(eventValue);
  }

  // public UltEvent<T> Response;

  // ! Cannot add component in inspector if put here
  // public class IntObserver : MonoParamObserver<int> { } 
}