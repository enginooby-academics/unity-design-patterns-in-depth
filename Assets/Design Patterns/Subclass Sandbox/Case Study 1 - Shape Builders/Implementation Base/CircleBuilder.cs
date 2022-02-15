using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

namespace SubclassSandboxPattern.Case1.Base {
  /// <summary>
  ///   * The 'Sandbox Subclass'
  /// </summary>
  public class CircleBuilder : Builder {
    [SerializeField] [OnValueChanged(nameof(Rebuild))] [Range(2f, 6f)]
    private float _radius = 4f;

    [SerializeField] [OnValueChanged(nameof(Rebuild))] [Range(5f, 20f)] [Tooltip("Number of points on the circle.")]
    private int _totalPoints = 10;

    [SerializeField] [OnValueChanged(nameof(Rebuild))] [EnumToggleButtons]
    private Axis _facingAxis = Axis.Z;

    protected override void Build() {
      // AddCube(Vector3.zero);

      for (var i = 0; i < _totalPoints; i++) {
        // distance around the circle
        var radian = 2 * Mathf.PI / _totalPoints * i;
        var dir1 = Mathf.Sin(radian);
        var dir2 = Mathf.Cos(radian);
        var spawnDir = _facingAxis switch {
          Axis.X => new Vector3(0, dir1, dir2),
          Axis.Y => new Vector3(dir1, 0, dir2),
          Axis.Z => new Vector3(dir1, dir2, 0),
          _ => throw new ArgumentOutOfRangeException(),
        };
        var pos = spawnDir * _radius;
        var point = AddCube(pos);
        if (i.IsEven()) ShakeRotation(point);
      }
    }
  }
}