using UnityEngine;
using Sirenix.OdinInspector;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CompositePattern.Case1.Base {
  // ? Rename to AreaArc3D or AreaSphericalSector
  [Serializable, InlineProperty]
  /// <summary>
  /// * Define area from radius & angle. Use case: vision area.
  /// </summary>
  public class AreaCircular : Area {
    // private string _label;

    [SerializeField]
    [BoxGroup("Circular")]
    private float _radius = 10f;

    [SerializeField]
    [BoxGroup("Circular")]
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
      if (!origin.HasValue) return;
      Vector3 originPos = origin.Value;
      Gizmos.color = _gizmosColor;
      Handles.color = _gizmosColor;
      Transform transform = origin.GameObject.transform;

      // TODO: draw portion of sphere based on angle
      // if (gizmosWire) Gizmos.DrawWireSphere(origin.GameObject.transform.position, radius);
      // else Gizmos.DrawSphere(origin.GameObject.transform.position, radius);

      if (_gizmosMode == GizmosMode.Solid) {
        Handles.DrawSolidArc(transform.position, transform.up, transform.position, _angle, _radius);
      } else if (_gizmosMode == GizmosMode.Wire) {
        Handles.DrawWireArc(transform.position, transform.up, transform.position, _angle, _radius);
      }
    }
  }
}