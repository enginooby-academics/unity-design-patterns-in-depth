using System.Collections.Generic;
using Enginooby.Utils;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Sirenix.Serialization;

#else
using Enginooby.Attribute;
using Enginooby.Core;
#endif

// ? Rename to CollectionVector3
[CreateAssetMenu(fileName = "New Area Point", menuName = "Area/Area Point", order = 0)]
public class AreaPointData : SerializedScriptableObject {
  [OdinSerialize] public List<Vector3> Points { get; private set; }

  // TODO:
  // + Normalize
  // + Round
  // + Scale X/Y/Z

  [Button]
  public void Scale(float value) {
    // ! ForEach pass by value, hence not changed
    for (var i = 0; i < Points.Count; i++) Points[i] *= value;
  }

  [Button]
  public void ScaleInverse(float value) {
    for (var i = 0; i < Points.Count; i++) Points[i] /= value;
  }

  public void DrawGizmos(Vector3 origin, float scale = 1f, Color? color = null) {
    if (Points.IsNullOrEmpty()) return;

    color ??= Color.red;
    Gizmos.color = color.Value;
    Points.ForEach(point => Gizmos.DrawSphere(point * scale + origin, .5f));
  }
}