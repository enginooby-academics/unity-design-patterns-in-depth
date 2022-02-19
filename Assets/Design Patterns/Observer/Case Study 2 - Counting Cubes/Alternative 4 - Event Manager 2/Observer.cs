using System;
using UnityEngine;

namespace ObserverPattern.Case2.Alternative4 {
  /// <summary>
  /// Observer base class.
  /// </summary>
  public class Observer : MonoBehaviour, IObserver {
    public void OnNotify(EventHandlerParam handlerParam, Action<EventHandlerParam> handler) {
      handler?.Invoke(handlerParam);
    }
  }

  public interface IObserver {
    // TODO: Use interface default method when Unity support
    // void OnNotify(EventHandlerParam handlerParam, Action<EventHandlerParam> handler);
  }
}