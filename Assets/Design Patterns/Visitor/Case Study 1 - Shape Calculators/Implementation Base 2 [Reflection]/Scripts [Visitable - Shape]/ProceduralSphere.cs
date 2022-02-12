using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

namespace VisitorPattern.Case1.Base2 {
  /// <summary>
  ///   * A 'Concrete Visitable Element' class
  /// </summary>
  public class ProceduralSphere : ProceduralShape {
    [SerializeField] [OnValueChanged(nameof(CreateMesh))] [Range(1f, 5f)]
    private float _radius;

    private readonly int _horizontalSegments = 16;
    private readonly int _verticalSegments = 16;
    public float Radius => _radius;

    protected override void CreateMeshData() {
      var horizontalSegmentAngle = 360f / _horizontalSegments;
      var verticalSegmentAngle = 180f / _verticalSegments;
      float currentVerticalAngle = -90;

      for (var y = 0; y <= _verticalSegments; y++) {
        var currentHorizontalAngle = 0f;
        for (var x = 0; x <= _horizontalSegments; x++) {
          var point = PointOnSpheroid(_radius, _radius, currentHorizontalAngle, currentVerticalAngle);
          vertices.Add(point);
          normals.Add(point.normalized);
          uv.Add(new Vector2((float) x / _horizontalSegments, (float) y / _verticalSegments));
          currentHorizontalAngle -= horizontalSegmentAngle;
        }

        currentVerticalAngle += verticalSegmentAngle;
      }

      // Extra vertices due to the uvmap seam
      var horizontalCount = _horizontalSegments + 1;
      for (var ring = 0; ring < _verticalSegments; ring++)
      for (var i = 0; i < horizontalCount - 1; i++) {
        var i0 = ring * horizontalCount + i;
        var i1 = (ring + 1) * horizontalCount + i;
        var i2 = ring * horizontalCount + i + 1;
        var i3 = (ring + 1) * horizontalCount + i + 1;

        triangles.Add(i0);
        triangles.Add(i1);
        triangles.Add(i2);
        triangles.Add(i2);
        triangles.Add(i1);
        triangles.Add(i3);
      }
    }

    private static Vector3 PointOnSpheroid(float radius, float height, float horizontalAngle, float verticalAngle) {
      var horizontalRadians = horizontalAngle * Mathf.Deg2Rad;
      var verticalRadians = verticalAngle * Mathf.Deg2Rad;
      var cosVertical = Mathf.Cos(verticalRadians);

      return new Vector3(
        radius * Mathf.Sin(horizontalRadians) * cosVertical,
        height * Mathf.Sin(verticalRadians),
        radius * Mathf.Cos(horizontalRadians) * cosVertical);
    }
  }
}