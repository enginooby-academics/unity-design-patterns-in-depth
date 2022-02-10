using Sirenix.OdinInspector;
using UnityEngine;
using static VectorUtils;
using static GeometryUtils;

namespace SubclassSandboxPattern.Case1.Base {
  /// <summary>
  /// * [The 'Sandbox Subclass'] 
  /// </summary>
  public class SquareBuilder : Builder {
    [SerializeField, OnValueChanged(nameof(Rebuild)), Range(2f, 6f)]
    private float _size = 4f;

    [SerializeField, OnValueChanged(nameof(Rebuild)), Range(0f, 10f)]
    [Tooltip("Number of points on each side excluding 2 corner points.")]
    private int _sidePoints = 2;

    [SerializeField, OnValueChanged(nameof(Rebuild)), Range(0f, 10f)]
    private int _diagonalPoints = 0;

    [SerializeField, OnValueChanged(nameof(Rebuild))]
    private Color _sideColor = Color.blue;

    [SerializeField, OnValueChanged(nameof(Rebuild))]
    private Color _cornerColor = Color.green;

    [SerializeField, OnValueChanged(nameof(Rebuild))]
    private Color _diagonalColor = Color.red;

    // IMPL
    // [SerializeField, OnValueChanged(nameof(Rebuild)), EnumToggleButtons]
    // private Axis _facingAxis = Axis.Z;

    protected override void Build() {
      // centroid point
      // var centroid = AddCube(Vector3.zero);

      // corner points
      var corner1 = _size * vm110;
      var corner2 = _size * v110;
      var corner3 = _size * v1m10;
      var corner4 = _size * vm1m10;
      ShakeScale(AddCube(corner1, color: _cornerColor));
      ShakeScale(AddCube(corner2, color: _cornerColor));
      ShakeScale(AddCube(corner3, color: _cornerColor));
      ShakeScale(AddCube(corner4, color: _cornerColor));

      // side points
      PositionsInBetween(corner1, corner2, _sidePoints).ForEach(pos => AddCube(pos, color: _sideColor));
      PositionsInBetween(corner2, corner3, _sidePoints).ForEach(pos => AddCube(pos, color: _sideColor));
      PositionsInBetween(corner3, corner4, _sidePoints).ForEach(pos => AddCube(pos, color: _sideColor));
      PositionsInBetween(corner4, corner1, _sidePoints).ForEach(pos => AddCube(pos, color: _sideColor));

      // diagonal points
      Vector3 centroidPos = transform.position;
      PositionsInBetween(centroidPos, corner1, _diagonalPoints / 2).ForEach(pos => AddCube(pos, color: _diagonalColor));
      PositionsInBetween(centroidPos, corner2, _diagonalPoints / 2).ForEach(pos => AddCube(pos, color: _diagonalColor));
      PositionsInBetween(centroidPos, corner3, _diagonalPoints / 2).ForEach(pos => AddCube(pos, color: _diagonalColor));
      PositionsInBetween(centroidPos, corner4, _diagonalPoints / 2).ForEach(pos => AddCube(pos, color: _diagonalColor));
    }
  }
}
