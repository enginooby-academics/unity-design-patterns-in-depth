// * Define area from axes (results in line/face/box) for spawner, detector, boundary...
// * basically a Vector3Range with position origins
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[Serializable, InlineProperty]
public class AreaAxis : SerializableBase, IArea {
  [LabelText("Area Origins")] public List<Reference> origins;

  [HideLabel] public Vector3Range box = new Vector3Range(title: "Axes", new Vector2(-100, 100));

  public bool Contains(Vector3 pos) {
    foreach (Reference origin in origins) {
      Vector3 originPos = origin.GameObject.transform.position;
      Vector3 diffPos = pos - originPos;
      if (box.ContainsIgnoreZero(diffPos)) return true;
    }

    return false;
  }

  public void DrawGizmos(Color? color = null) {
    origins.ForEach(DrawGizmosSingleOrigin);
  }

  private void DrawGizmosSingleOrigin(Reference origin) {
    if (!origin.GameObject) return;
    Vector3 originPos = origin.GameObject.transform.position;
    Gizmos.color = Color.magenta;

    Vector3 boxOriginPos = originPos + box.Average;
    if (box.xRange.IsZero) boxOriginPos.x = originPos.x;
    if (box.yRange.IsZero) boxOriginPos.y = originPos.y;
    if (box.zRange.IsZero) boxOriginPos.z = originPos.z;
    Vector3 boundarySize = box.Size;
    Gizmos.DrawWireCube(boxOriginPos, boundarySize);
  }

  /// <summary>
  /// Return a random position lie inside a "box" (determined by Vector3Range) of an origin. Return Vector3.zero if origin index is invalid.
  /// </summary>
  public Vector3 RandomByOrigin(int originIndex) {
    if (!origins.ValidateIndex(originIndex) || origins.IsUnset()) return Vector3.zero; // ? throw exception instead
    return origins[originIndex].GameObject.transform.position + box.Random;
  }

  /// <summary>
  /// Return a random position lie inside a "box" (determined by Vector3Range) of the first origin.
  /// </summary>
  public Vector3 Random {
    get => origins[0].GameObject.transform.position + box.Random;
  }
}