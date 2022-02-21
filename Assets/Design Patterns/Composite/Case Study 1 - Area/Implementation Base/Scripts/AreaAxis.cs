using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif

#if ASSET_ALINE
using Drawing;
#endif

// TODO: Exclusive area
namespace CompositePattern.Case1.Base {
  [Serializable]
  [InlineProperty]
  /// <summary>
  /// The 'Leaf' class.
  /// * Define area from axes (results in line/face/box).
  /// * Basically a Vector3Range with position origins.
  /// </summary>
  public class AreaAxis : Area {
    [HideLabel] [SerializeField] [ToggleGroup(nameof(_isEnabled))]
    protected Vector3Range _box = new("Axes", new Vector2(-100, 100));

    public AreaAxis() { }

    public AreaAxis(Vector3 staticOrigin) : base(staticOrigin) { }

    public AreaAxis(GameObject gameObjectOrigin) : base(gameObjectOrigin) { }


    /// <summary>
    ///   Return a random position lie inside a "box" (determined by Vector3Range) of a random origin.
    /// </summary>
    public override Vector3 RandomPoint => _origins.GetRandom().Value + _box.Random;

    public override bool Contains(Vector3 pos) {
      if (!_isEnabled) return false;

      foreach (var origin in _origins) {
        var diffPos = pos - origin.Value;
        if (_box.ContainsIgnoreZero(diffPos)) return true;
      }

      return false;
    }

    protected override void DrawGizmosOnSingleOrigin(ReferenceVector3 origin) {
      var center = GetBoxOriginPos(origin);
      var size = _box.Size;

#if ASSET_ALINE
      // REFACTOR
      if (_gizmosDisplay.HasFlag(GizmosDisplay.InGame))
        using (Draw.ingame.WithLineWidth(_gizmosWidth)) {
          if (_gizmosMode == GizmosMode.Solid)
            Draw.ingame.SolidBox(center, size, _gizmosColor);
          else if (_gizmosMode == GizmosMode.Wire) Draw.ingame.WireBox(center, size, _gizmosColor);
        }

      if (_gizmosDisplay.HasFlag(GizmosDisplay.OnGizmos))
        using (Draw.WithLineWidth(_gizmosWidth)) {
          if (_gizmosMode == GizmosMode.Solid)
            Draw.SolidBox(center, size, _gizmosColor);
          else if (_gizmosMode == GizmosMode.Wire) Draw.WireBox(center, size, _gizmosColor);
        }
#endif
    }

    private Vector3 GetBoxOriginPos(ReferenceVector3 origin) {
      var originPos = origin.Value;
      var boxOriginPos = originPos + _box.Average;
      if (_box.xRange.IsZero) boxOriginPos.x = originPos.x;
      if (_box.yRange.IsZero) boxOriginPos.y = originPos.y;
      if (_box.zRange.IsZero) boxOriginPos.z = originPos.z;

      return boxOriginPos;
    }
  }
}