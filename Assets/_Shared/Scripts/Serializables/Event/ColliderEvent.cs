// * Base class for events happening on interaction of Collider components such as Trigger/Collision

using System;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

namespace Enginooby {
  [Serializable]
  [InlineProperty]
  public class ColliderEvent : TriggerEvent {
    [PropertyOrder(-1)] public bool enableForAll;

    [DisableIf(nameof(enableForAll))] [PropertyOrder(-1)]
    public List<Reference> targets = new List<Reference>();

    public List<string> Tags {
      get {
        var tags = new List<string>();
        targets.ForEach(target => {
          if (target.findMethod == Reference.FindMethod.Tag) tags.Add(target.Tag);
        });
        return tags;
      }
    }

    /// <summary>
    ///   Check if the GameObject is valid by target list to invoke the event
    /// </summary>
    public bool IsTriggerBy(GameObject go) {
      if (enableForAll) return true;

      // TODO: valid ref/name/type
      return Tags.Contains(go.tag);
    }
  }
}