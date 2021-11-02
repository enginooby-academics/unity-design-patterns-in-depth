// * Base class for events happening on interaction of Collider components such as Trigger/Collision
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using UnityEngine.Events;


[Serializable, InlineProperty]
public class ColliderEvent : TriggerEvent {
  [SerializeField, PropertyOrder(-2)] public bool enableForAll = false;

  [DisableIf(nameof(enableForAll))]
  [SerializeField, PropertyOrder(-1)] public List<Reference> targets = new List<Reference>();

  public List<string> Tags {
    get {
      List<string> tags = new List<string>();
      targets.ForEach(target => {
        if (target.findMethod == Reference.FindMethod.Tag) tags.Add(target.tag);
      });
      return tags;
    }
  }

  /// <summary>
  /// Check if the GameObject is valid by target list to invoke the event
  /// </summary>
  public bool IsTriggerBy(GameObject go) {
    if (enableForAll) return true;

    // TODO: valid ref/name/type
    return Tags.Contains(go.tag);
  }
}