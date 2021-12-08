using Sirenix.OdinInspector;
using UnityEngine;
using static VectorUtils;
using static GeometryUtils;

namespace SubclassSandboxPattern.Case1.Base {
  public class SquareBuilder : Builder {
    [SerializeField, OnValueChanged(nameof(Rebuild)), Range(2f, 6f)]
    private float _size = 4f;

    [SerializeField, OnValueChanged(nameof(Rebuild)), Range(0f, 10f)]
    [Tooltip("Number of points on each side excluding 2 corner points.")]
    private int sidePoints = 2;

    // IMPL
    // [SerializeField, OnValueChanged(nameof(Rebuild)), EnumToggleButtons]
    // private Axis _facingAxis = Axis.Z;

    private Vector3 corner1, corner2, corner3, corner4;

    protected override void Build() {
      AddCube(Vector3.zero);

      // corner points
      corner1 = vm110 * _size;
      corner2 = v110 * _size;
      corner3 = v1m10 * _size;
      corner4 = vm1m10 * _size;
      AddCube(corner1);
      AddCube(corner2);
      AddCube(corner3);
      AddCube(corner4);

      // side points
      PositionsInBetween(corner1, corner2, sidePoints).ForEach(pos => AddCube(pos));
      PositionsInBetween(corner2, corner3, sidePoints).ForEach(pos => AddCube(pos));
      PositionsInBetween(corner3, corner4, sidePoints).ForEach(pos => AddCube(pos));
      PositionsInBetween(corner4, corner1, sidePoints).ForEach(pos => AddCube(pos));
    }
  }
}
