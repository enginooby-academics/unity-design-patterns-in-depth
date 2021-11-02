using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;


[Serializable, InlineProperty]
public abstract class SerializableBase {
  /// <summary>
  /// GameObject of the component containing this Serializable class.
  /// </summary>
  [HideInInspector]
  [OnValueChanged(nameof(OnComponentOwnerChange))]
  public GameObject componentOwner;

  /// <summary>
  /// Need to invoke in Reset() of the GameObject.
  /// </summary>
  public virtual void SetComponentOwner(GameObject componentOwner) {
    this.componentOwner = componentOwner;
    OnComponentOwnerChange();
  }

  protected virtual void OnComponentOwnerChange() { }
}