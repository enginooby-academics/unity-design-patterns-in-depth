using UnityEngine;
using UnityEngine.Events;

namespace ObserverPattern.Case2.Alternative2 {
  public class OneVarObserver<T> : MonoBehaviour {
    public OneVarEventSO<T> Event;
    // public UltEvent<T> Response;
    public UnityEvent<T> Response;

    private void OnEnable() => Event.AddListener(this);

    private void OnDisable() => Event.RemoveListener(this);

    public void OnNotified(T eventValue) => Response.Invoke(eventValue);
  }

  // ! Cannot add component in inspector if put here
  // public class IntObserver : OneVarObserver<int> { } 
}