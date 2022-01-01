using System;
using UnityEngine;

namespace ObserverPattern.Case2.Alternative1 {
  public class ReferenceVariableSO<T> : ScriptableObject where T : IEquatable<T> {
    public T Value;
    private T _lastValue;

    /// <summary>
    /// [Update-safe method] <br/>
    /// Only perform the given action on value changed.
    /// </summary>
    public void PerformOnValueChanged(Action<T> action) {
      if (Value.Equals(_lastValue)) return;

      action.Invoke(Value);
      _lastValue = Value;
    }
  }
}