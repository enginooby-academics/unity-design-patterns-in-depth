#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#else
using Enginoobz.Attribute;
using Enginoobz.Core;
#endif

using UnityEngine;
using System.Collections.Generic;

// ? Rename to CollectionVector3
[CreateAssetMenu(fileName = "New Area Point", menuName = "Area/Area Point", order = 0)]
public class AreaPointData : SerializedScriptableObject {
  [OdinSerialize]
  public List<Vector3> Points { get; private set; }

  // TODO:
  // + Normalize
  // + Round
  // + Scale X/Y/Z

  [Button]
  public void Scale(float value) {
    // ! ForEach pass by value, hence not changed
    for (int i = 0; i < Points.Count; i++) {
      Points[i] *= value;
    }
  }

  [Button]
  public void ScaleInverse(float value) {
    for (int i = 0; i < Points.Count; i++) {
      Points[i] /= value;
    }
  }

  public void DrawGizmos(Vector3 origin, float scale = 1f, Color? color = null) {
    if (Points.IsUnset()) return;

    color ??= Color.red;
    Gizmos.color = color.Value;
    Points.ForEach(point => Gizmos.DrawSphere(point * scale + origin, .5f));
  }
}