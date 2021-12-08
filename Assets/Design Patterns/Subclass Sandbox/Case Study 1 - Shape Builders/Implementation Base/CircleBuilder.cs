using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SubclassSandboxPattern.Case1.Base {
  /// <summary>
  /// * [The 'Sandbox' subclass] 
  /// Couple with the base class only
  /// </summary>
  public class CircleBuilder : Builder {
    [SerializeField, OnValueChanged(nameof(Rebuild)), Range(2f, 6f)]
    private float _radius = 4f;

    [SerializeField, OnValueChanged(nameof(Rebuild)), Range(5f, 20f)]
    [Tooltip("Number of points on the circle.")]
    private int totalPoints = 10;

    [SerializeField, OnValueChanged(nameof(Rebuild)), EnumToggleButtons]
    private Axis _facingAxis = Axis.Z;

    protected override void Build() {
      for (int i = 0; i < totalPoints; i++) {
        // distance around the circle
        var radian = 2 * Mathf.PI / totalPoints * i;
        var dir1 = Mathf.Sin(radian);
        var dir2 = Mathf.Cos(radian);
        var spawnDir = _facingAxis switch
        {
          Axis.X => new Vector3(0, dir1, dir2),
          Axis.Y => new Vector3(dir1, 0, dir2),
          Axis.Z => new Vector3(dir1, dir2, 0),
          _ => throw new ArgumentOutOfRangeException()
        };
        var spawnPos = spawnDir * _radius;
        AddCube(spawnPos, name: dir1.ToDegree() + " " + dir2.ToDegree() + " " + spawnPos);
      }
    }
  }
}
