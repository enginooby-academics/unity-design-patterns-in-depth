using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Drawing;
using Unity.Mathematics;
#if UNITY_EDITOR
#endif

namespace CompositePattern.Case1.Base {
  // ? Rename to AreaArc3D or AreaSphericalSector
  [Serializable, InlineProperty]
  /// <summary>
  /// The 'Leaf' class.
  /// * Define area from radius & angle. Use case: vision area.
  /// </summary>
  public class AreaCircular : Area {
    // private string _label;

    [SerializeField]
    [ToggleGroup(nameof(_isEnabled))]
    [BoxGroup(nameof(_isEnabled) + "/Circular")]
    private float _radius = 10f;

    [SerializeField]
    [ToggleGroup(nameof(_isEnabled))]
    [BoxGroup(nameof(_isEnabled) + "/Circular")]
    private float _angle = 360f;

    public AreaCircular() : base() {
    }

    public AreaCircular(Vector3 staticOrigin, float radius = 10f, float angle = 360f) : base(staticOrigin) {
      this._radius = radius;
      this._angle = angle;
    }

    public AreaCircular(GameObject gameObjectOrigin, float radius = 10f, float angle = 360f) : base(gameObjectOrigin) {
      this._radius = radius;
      this._angle = angle;
    }

    public override Vector3 RandomPoint => throw new NotImplementedException();

    public override bool Contains(Vector3 pos) {
      if (!_isEnabled) return false;

      foreach (ReferenceVector3 origin in _origins) {
        // https://learn.unity.com/tutorial/chasing-the-player?uv=2019.4&projectId=5e0b85cdedbc2a144cf5cde5#5e0b8be8edbc2a035d135cd8
        Vector3 directionToPos = pos - origin.Value;
        // TODO: resolve forward value for ReferenceVector3
        float angleToPos = Vector3.Angle(directionToPos, origin.GameObject.transform.forward);
        if (directionToPos.magnitude < _radius && angleToPos < _angle) return true;
      }

      return false;
    }

    protected override void DrawGizmosOnSingleOrigin(ReferenceVector3 origin) {
      // TODO: draw sphere sector (3D)
      float a1 = -.5f * _angle * Mathf.PI / 180f;
      float a2 = .5f * _angle * Mathf.PI / 180f;
      var arcStart = (float3)origin.Value + new float3(Mathf.Cos(a1), 0, Mathf.Sin(a1)) * _radius;
      var arcEnd = (float3)origin.Value + new float3(Mathf.Cos(a2), 0, Mathf.Sin(a2)) * _radius;

      using (Draw.WithLineWidth(_gizmosWidth)) {
        if (_gizmosMode == GizmosMode.Solid) {
          Draw.SolidArc(origin.Value, arcStart, arcEnd, _gizmosColor);
        } else if (_gizmosMode == GizmosMode.Wire) {
          Draw.Arc(origin.Value, arcStart, arcEnd, _gizmosColor);
        }
      }
    }
  }
}