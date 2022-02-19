using System;
using System.Collections.Generic;
using System.Linq;

namespace ObserverPattern.Case2.Alternative4 {
  // observer and event handlers
  using Handlers = Dictionary<Action<EventHandlerParam>, IObserver>;

  public class EventHandlerParam {
    public object Event;
    public object Param;
  }

  public class EventManager : MonoBehaviourSingleton<EventManager> {
    // custom event & handlers
    private readonly Dictionary<object, Handlers> _events = new Dictionary<object, Handlers>();

    public void AddObserver(object @event, IObserver observer, Action<EventHandlerParam> handler) {
      if (_events.TryGetValue(@event, out var handlers))
        handlers.Add(handler, observer);
      else
        _events.Add(@event, new Handlers {{handler, observer}});
    }

    public void NotifyEvent(object @event, object param = null) {
      if (!_events.TryGetValue(@event, out var handlers)) return;

      var eventHandlerParam = new EventHandlerParam {
        Param = param,
        Event = @event,
      };

      foreach (var handler in handlers.Keys) handler.Invoke(eventHandlerParam);
      // for (var i = 0; i < handlers.Count; i++) {
      //   var observer = handlers.Values.ElementAt(i);
      //   observer.OnNotify(eventHandlerParam, handlers.Keys.ElementAt(i));
      // }
    }

    public void RemoveHandler(object @event, IObserver observer, Action<EventHandlerParam> handler) {
      if (!_events.TryGetValue(@event, out var handlers)) return;

      handlers.Remove(handler);
      // for (var i = 0; i < handlers.Count; i++) {
      //   if (handlers.Keys.ElementAt(i) == handler && handlers.Values.ElementAt(i) == observer) {
      //     handlers.Remove(handler);
      //   }
      // }
    }

    public void RemoveAllHandlers(IObserver observer) {
      foreach (var @event in _events
                 .Select(item => item.Value)
                 .Where(actions => actions.ContainsValue(observer)))
        for (var i = 0; i < @event.Count; i++)
          if (@event.Values.ElementAt(i) == observer)
            @event.Remove(@event.Keys.ElementAt(i));
    }

    private void OnApplicationQuit() {
      foreach (var observer in _events.Values.SelectMany(handlers => handlers.Values).ToList())
        RemoveAllHandlers(observer);
    }
  }
}