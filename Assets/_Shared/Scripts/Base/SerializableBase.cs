using UnityEngine;
using Sirenix.OdinInspector;
using System;

[Serializable, InlineProperty]
public abstract class SerializableBase {
  /// <summary>
  /// GameObject of the component containing this Serializable class.
  /// </summary>
  [HideInInspector]
  [OnValueChanged(nameof(OnGameObjectChanged))]
  public GameObject GameObject;

  /// <summary>
  /// Invoke in Reset() of the MonoBehaviour.
  /// </summary>
  public virtual void SetGameObject(GameObject gameObject) {
    this.GameObject = gameObject;
    OnGameObjectChanged();
  }

  protected virtual void OnGameObjectChanged() { }
}