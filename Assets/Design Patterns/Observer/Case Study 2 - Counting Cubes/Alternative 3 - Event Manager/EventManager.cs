using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// ? What is the point of EventManager when we can just use Action
namespace ObserverPattern.Case2.Alternative3 {
  using EventHandler = Action<Dictionary<string, object>>; // paramName + value

  public class EventManager : MonoBehaviourSingleton<EventManager> {
    private readonly Dictionary<Event, EventHandler> _events = new();

    private void OnApplicationQuit() {
      // foreach loop causes error on modifying
      for (var i = 0; i < _events.Count; i++) {
        var @event = _events.ElementAt(i);
        RemoveHandler(@event.Key, @event.Value);
      }
    }

    public void AddEventHandler(Event @event, EventHandler handler) {
      if (_events.TryGetValue(@event, out var cachedEvent)) {
        cachedEvent += handler;
        _events[@event] = cachedEvent;
      }
      else {
        cachedEvent += handler;
        _events.Add(@event, cachedEvent);
      }
    }

    public void AddEventHandler<T>(Event<T> eventId, Action<T> method) {
      void HandleEvent(Dictionary<string, object> message) {
        var paramValue = (T) message[eventId.Param];
        method.Invoke(paramValue);
      }

      AddEventHandler(eventId, HandleEvent);
    }

    public void RemoveHandler(Event @event, EventHandler handler) {
      if (!_events.TryGetValue(@event, out var cachedEvent)) return;

      cachedEvent -= handler;
      _events[@event] = cachedEvent;
    }

    public static void NotifyEvent<T>(Event<T> @event, T value) {
      if (Instance._events.TryGetValue(@event, out var cachedEvent)) {
        var message = new Dictionary<string, object> {{@event.Param, value}};
        cachedEvent.Invoke(message);
      }
    }

    public void NotifyEvent(Event @event, Dictionary<string, object> message) {
      if (_events.TryGetValue(@event, out var cachedEvent))
        cachedEvent.Invoke(message);
    }
  }

  public class Event { }

  [Serializable]
  [InlineProperty]
  public class Event<T0> : Event {
    [HideInInspector] public string Param = "";

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