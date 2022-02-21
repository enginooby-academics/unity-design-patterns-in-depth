using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class ScriptableObjectBase : ScriptableObject {
  /// <summary>
  ///   This will be executed for every existing SO, even if it's not referenced in the active scene.
  /// </summary>
  protected virtual void OnPlayModeEnter() { }

  /// <summary>
  ///   This will be executed for every existing SO, even if it's not referenced in the active scene.
  /// </summary>
  protected virtual void OnPlayModeExit() { }

  /// <summary>
  ///   Executed on both OnPlayModeEnter and OnPlayModeExit. <br />
  ///   This will be executed for every existing SO, even if it's not referenced in the active scene.
  /// </summary>
  protected virtual void OnPlayModeChange() { }

#if UNITY_EDITOR
  protected void OnEnable() {
    EditorApplication.playModeStateChanged += OnPlayStateChange;
  }

  protected void OnDisable() {
    EditorApplication.playModeStateChanged -= OnPlayStateChange;
  }

  private void OnPlayStateChange(PlayModeStateChange state) {
    if (state == PlayModeStateChange.EnteredPlayMode)
      OnPlayModeEnter();
    else if (state == PlayModeStateChange.ExitingPlayMode) OnPlayModeExit();
    OnPlayModeChange();
  }
#else
  protected void OnEnable() => OnPlayModeEnter();
  protected void OnDisable() => OnPlayModeExit();
#endif
}