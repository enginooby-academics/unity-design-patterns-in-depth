using System;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

// TODO: Move to Library
// + Implement static value (!= ref value)

// Link to Library/Core/Reference
namespace ObserverPattern.Case2.Alternative1 {
  [InitializeOnLoad]
  public class ReferenceVariableSO<T> : ScriptableObject where T : IEquatable<T> {
    [SerializeField, OnValueChanged(nameof(SetToInitValue))]
    private bool _isValuePersistent = true;
    [SerializeField, OnValueChanged(nameof(SetToInitValue))]
    private T _initialValue;
    [DisplayAsString, LabelText("Current Value")]
    public T Value;

    private T _lastValue;

    private void SetToInitValue() => Value = _initialValue;

    /// <summary>
    /// [Update-safe method] <br/>
    /// Only perform the given action on value changed.
    /// </summary>
    public void PerformOnValueChanged(Action<T> action) {
      if (Value.Equals(_lastValue)) return;

      action.Invoke(Value);
      _lastValue = Value;
    }

    private void OnAppStart() {
      if (!_isValuePersistent) SetToInitValue();
      _lastValue = _initialValue;
    }

    private void OnAppQuit() {
      if (!_isValuePersistent) SetToInitValue();
      _lastValue = _initialValue;
    }

#if UNITY_EDITOR
    protected void OnEnable() {
      EditorApplication.playModeStateChanged += OnPlayStateChange;
    }

    protected void OnDisable() {
      EditorApplication.playModeStateChanged -= OnPlayStateChange;
    }

    void OnPlayStateChange(PlayModeStateChange state) {
      if (state == PlayModeStateChange.EnteredPlayMode) {
        OnAppStart();
      } else if (state == PlayModeStateChange.ExitingPlayMode) {
        OnAppQuit();
      }
    }
#else
        protected void OnEnable() => OnAppStart();
        protected void OnDisable() => OnAppQuit();
#endif
  }
}