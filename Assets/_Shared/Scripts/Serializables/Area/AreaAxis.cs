// * Define area from axes (results in line/face/box)
// * basically a Vector3Range with position origins

using System;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

[Serializable]
[InlineProperty]
public class AreaAxis : SerializableBase, IArea {
  [LabelText("Area Origins")] public List<Reference> origins;

  [HideLabel] public Vector3Range box = new Vector3Range("Axes", new Vector2(-100, 100));

  /// <summary>
  ///   Return a random position lie inside a "box" (determined by Vector3Range) of the first origin.
  /// </summary>
  public Vector3 Random => origins[0].GameObject.transform.position + box.Random;

  public bool Contains(Vector3 pos) {
    foreach (var origin in origins) {
      var originPos = origin.GameObject.transform.position;
      var diffPos = pos - originPos;
      if (box.ContainsIgnoreZero(diffPos)) return true;
    }

    return false;
  }

  public void DrawGizmos(Color? color = null) {
    origins.ForEach(DrawGizmosSingleOrigin);
  }

  private void DrawGizmosSingleOrigin(Reference origin) {
    if (!origin.GameObject) return;
    var originPos = origin.GameObject.transform.position;
    Gizmos.color = Color.magenta;

    var boxOriginPos = originPos + box.Average;
    if (box.xRange.IsZero) boxOriginPos.x = originPos.x;
    if (box.yRange.IsZero) boxOriginPos.y = originPos.y;
    if (box.zRange.IsZero) boxOriginPos.z = originPos.z;
    var boundarySize = box.Size;
    Gizmos.DrawWireCube(boxOriginPos, boundarySize);
  }

  /// <summary>
  ///   Return a random position lie inside a "box" (determined by Vector3Range) of an origin. Return Vector3.zero if origin
  ///   index is invalid.
  /// </summary>
  public Vector3 RandomByOrigin(int originIndex) {
    if (!origins.HasIndex(originIndex) || origins.IsUnset()) return Vector3.zero; // ? throw exception instead
    return origins[originIndex].GameObject.transform.position + box.Random;
  }
}