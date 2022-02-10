using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

// ? What is the point of EventManager when we can just use Action
namespace ObserverPattern.Case2.Alternative3 {
  using ObservingAction = Action<Dictionary<string, object>>; // paramName + value
  public class EventManager : MonoBehaviourSingleton<EventManager> {
    private Dictionary<EventBase, ObservingAction> events = new Dictionary<EventBase, ObservingAction>();

    public static void StartListening(EventBase eventId, ObservingAction observingAction) {
      if (Instance.events.TryGetValue(eventId, out var @event)) {
        @event += observingAction;
        Instance.events[eventId] = @event;
      } else {
        @event += observingAction;
        Instance.events.Add(eventId, @event);
      }
    }

    public static void StartListening<T>(EventBase<T> eventId, Action<T> method) {
      void ObservingAction(Dictionary<string, object> message) {
        var paramValue = (T)message[eventId.Param];
        method.Invoke(paramValue);
      }

      StartListening(eventId, ObservingAction);
    }

    public static void StopListening(EventBase eventId, ObservingAction observingAction) {
      if (Instance.events.TryGetValue(eventId, out var @event)) {
        @event -= observingAction;
        Instance.events[eventId] = @event;
      }
    }

    private void OnApplicationQuit() {
      // foreach loop causes error on modifying
      for (int i = 0; i < Instance.events.Count; i++) {
        var @event = Instance.events.ElementAt(i);
        StopListening(@event.Key, @event.Value);
      }
    }

    public static void TriggerEvent<T>(EventBase<T> eventId, T value) {
      if (Instance.events.TryGetValue(eventId, out var @event)) {
        var message = new Dictionary<string, object> { { eventId.Param, value } };
        @event.Invoke(message);
      }
    }

    public static void TriggerEvent(EventBase eventId, Dictionary<string, object> message) {
      if (Instance.events.TryGetValue(eventId, out var @event)) {
        @event.Invoke(message);
      }
    }
  }

  public class EventBase {
  }

  [Serializable, InlineProperty]
  public class EventBase<T0> : EventBase {
    [HideInInspector]
    public string Param = "";

    // [SerializeField]
    // private UnityEvent<T0> _unityEvent = new UnityEvent<T0>();
    // public UnityEvent<T0> UnityEvent => _unityEvent;

    // [Button]
    // public void InspectUnityEvent() {
    //   UnityEvent.GetPersistentEventCount().Log();
    //   UnityEvent.GetPersistentMethodName(0).Log();
    //   UnityEvent.GetPersistentTarget(0).Log();
    // }
  }
}