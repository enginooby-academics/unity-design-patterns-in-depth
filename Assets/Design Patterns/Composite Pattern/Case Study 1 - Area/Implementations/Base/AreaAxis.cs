using UnityEngine;
using Sirenix.OdinInspector;
using System;

// TODO: Exclusive area
namespace CompositePattern.Case1.Base {
  [Serializable, InlineProperty]
  /// <summary>
  /// * Define area from axes (results in line/face/box).
  /// * Basically a Vector3Range with position origins.
  /// </summary>
  public class AreaAxis : Area {
    public AreaAxis() : base() {
    }

    public AreaAxis(Vector3 staticOrigin) : base(staticOrigin) {
    }

    public AreaAxis(GameObject gameObjectOrigin) : base(gameObjectOrigin) {
    }

    [HideLabel]
    [SerializeField]
    protected Vector3Range _box = new Vector3Range(title: "Axes", new Vector2(-100, 100));

    public override bool Contains(Vector3 pos) {
      foreach (ReferenceVector3 origin in _origins) {
        Vector3 diffPos = pos - origin.Value;
        if (_box.ContainsIgnoreZero(diffPos)) return true;
      }

      return false;
    }

    protected override void DrawGizmosOnSingleOrigin(ReferenceVector3 origin) {
      if (!origin.HasValue) return;
      Vector3 originPos = origin.Value;
      Gizmos.color = _gizmosColor;

      Vector3 boxOriginPos = originPos + _box.Average;
      if (_box.xRange.IsZero) boxOriginPos.x = originPos.x;
      if (_box.yRange.IsZero) boxOriginPos.y = originPos.y;
      if (_box.zRange.IsZero) boxOriginPos.z = originPos.z;
      Vector3 boundarySize = _box.Size;

      if (_gizmosMode == GizmosMode.Solid) {
        Gizmos.DrawCube(boxOriginPos, boundarySize);
      } else if (_gizmosMode == GizmosMode.Wire) {
        Gizmos.DrawWireCube(boxOriginPos, boundarySize); // TODO: Thickness
      }
    }

    /// <summary>
    /// Return a random position lie inside a "box" (determined by Vector3Range) of a random origin.
    /// </summary>
    public override Vector3 RandomPoint {
      get => _origins.GetRandom().Value + _box.Random;
    }
  }
}