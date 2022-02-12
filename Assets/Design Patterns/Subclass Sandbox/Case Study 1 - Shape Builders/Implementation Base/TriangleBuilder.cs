#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

// ! Utils below are core, not likely to change so can couple w/ sandbox subclasses
using UnityEngine;
using static VectorUtils;
using static GeometryUtils;

namespace SubclassSandboxPattern.Case1.Base {
  /// <summary>
  ///   * [The 'Sandbox Subclass']
  /// </summary>
  public class TriangleBuilder : Builder {
    [SerializeField] [OnValueChanged(nameof(Rebuild))] [Range(2f, 6f)]
    private float _height = 4f;

    [SerializeField]
    [OnValueChanged(nameof(Rebuild))]
    [Range(0f, 10f)]
    [Tooltip("Number of points on each side excluding 2 corner points.")]
    private int _sidePoints = 2;

    [SerializeField] [OnValueChanged(nameof(Rebuild))]
    private Color _sideColor = Color.blue;

    [SerializeField] [OnValueChanged(nameof(Rebuild))]
    private Color _cornerColor = Color.green;

    // [SerializeField, OnValueChanged(nameof(Rebuild)), EnumToggleButtons]
    // private Axis _facingAxis = Axis.Z;

    protected override void Build() {
      // centroid point
      // var centroid = AddCube(Vector3.zero);

      // corner points
      var corner1 = _height * v010;
      var corner2 = _height * vm1m10;
      var corner3 = _height * v1m10;
      AddCube(corner1, color: _cornerColor);
      AddCube(corner2, color: _cornerColor);
      AddCube(corner3, color: _cornerColor);

      // side points
      PositionsInBetween(corner1, corner2, _sidePoints).ForEach(point => AddCube(point, color: _sideColor));
      PositionsInBetween(corner2, corner3, _sidePoints).ForEach(point => AddCube(point, color: _sideColor));
      PositionsInBetween(corner3, corner1, _sidePoints).ForEach(point => AddCube(point, color: _sideColor));
    }
  }
}