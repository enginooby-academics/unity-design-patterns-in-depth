using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

// TODO: constraint value type (struct?)
// + Implement constant value (!= ref value)
public class ReferenceVariableSO<T> : ScriptableObjectBase where T : IEquatable<T> {
  [SerializeField] [OnValueChanged(nameof(ResetValue))]
  private bool _isValuePersistent = true;

  [SerializeField] [OnValueChanged(nameof(ResetValue))]
  private T _initialValue;

  [DisplayAsString] [LabelText("Current Value")]
  public T Value;

  private T _lastValue;

  private void ResetValue() {
    Value = _lastValue = _initialValue;
  }

  /// <summary>
  ///   [Update-safe method] <br />
  ///   Only perform the given action on value changed.
  /// </summary>
  public void PerformOnValueChanged(Action<T> action) {
    if (Value.Equals(_lastValue)) return;

    _lastValue = Value;
    action?.Invoke(Value);
  }

  protected override void OnPlayModeChange() {
    if (!_isValuePersistent) ResetValue();
  }
}