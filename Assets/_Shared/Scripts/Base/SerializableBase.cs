using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

[Serializable]
[InlineProperty]
public abstract class SerializableBase {
  /// <summary>
  ///   GameObject of the component containing this Serializable class.
  /// </summary>
  [HideInInspector] [OnValueChanged(nameof(OnGameObjectChanged))]
  public GameObject GameObject;

  /// <summary>
  ///   Invoke in Reset() of the MonoBehaviour.
  /// </summary>
  public virtual void SetGameObject(GameObject gameObject) {
    GameObject = gameObject;
    OnGameObjectChanged();
  }

  protected virtual void OnGameObjectChanged() {
  }
}