// UltEvents // Copyright 2021 Kybernetik //

using System;
using UnityEngine;

namespace UltEvents {
    /// <summary>
    ///   An event that takes a single <see cref="Collider2D" /> parameter.
    /// </summary>
    [Serializable]
  public sealed class TriggerEvent2D : UltEvent<Collider2D> {
  }

  /************************************************************************************************************************/

  /// <summary>
  ///   Holds <see cref="UltEvent" />s which are called by various <see cref="MonoBehaviour" /> 2D trigger events:
  ///   <see cref="OnTriggerEnter2D" />, <see cref="OnTriggerStay2D" />, and <see cref="OnTriggerExit2D" />.
  /// </summary>
  [AddComponentMenu(UltEventUtils.ComponentMenuPrefix + "Trigger Events 2D")]
  [HelpURL(UltEventUtils.APIDocumentationURL + "/TriggerEvents2D")]
  [DisallowMultipleComponent]
  [RequireComponent(typeof(Collider2D))]
  public class TriggerEvents2D : MonoBehaviour {
    /************************************************************************************************************************/

    [SerializeField] private TriggerEvent2D _TriggerEnterEvent;

    /************************************************************************************************************************/

    [SerializeField] private TriggerEvent2D _TriggerStayEvent;

    /************************************************************************************************************************/

    [SerializeField] private TriggerEvent2D _TriggerExitEvent;

    /// <summary>Invoked by <see cref="OnTriggerEnter2D" />.</summary>
    public TriggerEvent2D TriggerEnterEvent {
      get {
        if (_TriggerEnterEvent == null)
          _TriggerEnterEvent = new TriggerEvent2D();
        return _TriggerEnterEvent;
      }
      set => _TriggerEnterEvent = value;
    }

    /// <summary>Invoked by <see cref="OnTriggerStay2D" />.</summary>
    public TriggerEvent2D TriggerStayEvent {
      get {
        if (_TriggerStayEvent == null)
          _TriggerStayEvent = new TriggerEvent2D();
        return _TriggerStayEvent;
      }
      set => _TriggerStayEvent = value;
    }

    /// <summary>Invoked by <see cref="OnTriggerExit2D" />.</summary>
    public TriggerEvent2D TriggerExitEvent {
      get {
        if (_TriggerExitEvent == null)
          _TriggerExitEvent = new TriggerEvent2D();
        return _TriggerExitEvent;
      }
      set => _TriggerExitEvent = value;
    }

    /// <summary>Invokes <see cref="TriggerEnterEvent" />.</summary>
    public virtual void OnTriggerEnter2D(Collider2D collider) {
      if (_TriggerEnterEvent != null)
        _TriggerEnterEvent.Invoke(collider);
    }

    /// <summary>Invokes <see cref="TriggerExitEvent" />.</summary>
    public virtual void OnTriggerExit2D(Collider2D collider) {
      if (_TriggerExitEvent != null)
        _TriggerExitEvent.Invoke(collider);
    }

    /// <summary>Invokes <see cref="TriggerStayEvent" />.</summary>
    public virtual void OnTriggerStay2D(Collider2D collider) {
      if (_TriggerStayEvent != null)
        _TriggerStayEvent.Invoke(collider);
    }

    /************************************************************************************************************************/
  }
}