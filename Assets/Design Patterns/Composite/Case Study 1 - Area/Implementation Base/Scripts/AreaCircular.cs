using System;
using Unity.Mathematics;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif

#if ASSET_ALINE
using Drawing;
#endif

namespace CompositePattern.Case1.Base {
  // ? Rename to AreaArc3D or AreaSphericalSector
  [Serializable]
  [InlineProperty]
  /// <summary>
  /// The 'Leaf' class.
  /// * Define area from radius & angle. Use case: vision area.
  /// </summary>
  public class AreaCircular : Area {
    // private string _label;

    [SerializeField] [ToggleGroup(nameof(_isEnabled))] [BoxGroup(nameof(_isEnabled) + "/Circular")]
    private float _radius = 10f;

    [SerializeField] [ToggleGroup(nameof(_isEnabled))] [BoxGroup(nameof(_isEnabled) + "/Circular")]
    private float _angle = 360f;

    public AreaCircular() { }

    public AreaCircular(Vector3 staticOrigin, float radius = 10f, float angle = 360f) : base(staticOrigin) {
      _radius = radius;
      _angle = angle;
    }

    public AreaCircular(GameObject gameObjectOrigin, float radius = 10f, float angle = 360f) : base(gameObjectOrigin) {
      _radius = radius;
      _angle = angle;
    }

    public override Vector3 RandomPoint => throw new NotImplementedException();

    // FIX: range not correct
    public override bool Contains(Vector3 pos) {
      if (!_isEnabled) return false;

      foreach (var origin in _origins) {
        // https://learn.unity.com/tutorial/chasing-the-player?uv=2019.4&projectId=5e0b85cdedbc2a144cf5cde5#5e0b8be8edbc2a035d135cd8
        var directionToPos = pos - origin.Value;
        // TODO: resolve forward value for ReferenceVector3
        var angleToPos = Vector3.Angle(directionToPos, origin.GameObject.transform.forward);
        if (directionToPos.magnitude < _radius && angleToPos < _angle) return true;
      }

      return false;
    }

    protected override void DrawGizmosOnSingleOrigin(ReferenceVector3 origin) {
      // TODO: draw sphere sector (3D)
      // float a1 = -.5f * _angle * Mathf.PI / 180f;
      // float a2 = .5f * _angle * Mathf.PI / 180f;
      var a1 = 0 * _angle * Mathf.PI / 180f;
      var a2 = 1 * _angle * Mathf.PI / 180f;
      var arcStart = (float3) origin.Value + new float3(Mathf.Cos(a1), 0, Mathf.Sin(a1)) * _radius;
      var arcEnd = (float3) origin.Value + new float3(Mathf.Cos(a2), 0, Mathf.Sin(a2)) * _radius;

#if ASSET_ALINE
      using (Draw.WithLineWidth(_gizmosWidth)) {
        if (_gizmosMode == GizmosMode.Solid)
          Draw.SolidArc(origin.Value, arcStart, arcEnd, _gizmosColor);
        else if (_gizmosMode == GizmosMode.Wire) Draw.Arc(origin.Value, arcStart, arcEnd, _gizmosColor);
      }
#endif
    }
  }
}