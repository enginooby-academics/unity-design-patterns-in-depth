#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;
using System;
using Drawing;

// TODO: Exclusive area
namespace CompositePattern.Case1.Base {
  [Serializable, InlineProperty]
  /// <summary>
  /// The 'Leaf' class.
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
    [ToggleGroup(nameof(_isEnabled))]
    protected Vector3Range _box = new Vector3Range(title: "Axes", new Vector2(-100, 100));

    public override bool Contains(Vector3 pos) {
      if (!_isEnabled) return false;

      foreach (ReferenceVector3 origin in _origins) {
        Vector3 diffPos = pos - origin.Value;
        if (_box.ContainsIgnoreZero(diffPos)) return true;
      }

      return false;
    }

    protected override void DrawGizmosOnSingleOrigin(ReferenceVector3 origin) {
      Vector3 center = GetBoxOriginPos(origin);
      Vector3 size = _box.Size;

      // REFACTOR
      if (_gizmosDisplay.HasFlag(GizmosDisplay.InGame)) {
        using (Draw.ingame.WithLineWidth(_gizmosWidth)) {
          if (_gizmosMode == GizmosMode.Solid) {
            Draw.ingame.SolidBox(center, size, _gizmosColor);
          } else if (_gizmosMode == GizmosMode.Wire) {
            Draw.ingame.WireBox(center, size, _gizmosColor);
          }
        }
      }

      if (_gizmosDisplay.HasFlag(GizmosDisplay.OnGizmos)) {
        using (Draw.WithLineWidth(_gizmosWidth)) {
          if (_gizmosMode == GizmosMode.Solid) {
            Draw.SolidBox(center, size, _gizmosColor);
          } else if (_gizmosMode == GizmosMode.Wire) {
            Draw.WireBox(center, size, _gizmosColor);
          }
        }
      }
    }

    private Vector3 GetBoxOriginPos(ReferenceVector3 origin) {
      Vector3 originPos = origin.Value;
      Vector3 boxOriginPos = originPos + _box.Average;
      if (_box.xRange.IsZero) boxOriginPos.x = originPos.x;
      if (_box.yRange.IsZero) boxOriginPos.y = originPos.y;
      if (_box.zRange.IsZero) boxOriginPos.z = originPos.z;

      return boxOriginPos;
    }


    /// <summary>
    /// Return a random position lie inside a "box" (determined by Vector3Range) of a random origin.
    /// </summary>
    public override Vector3 RandomPoint {
      get => _origins.GetRandom().Value + _box.Random;
    }
  }
}