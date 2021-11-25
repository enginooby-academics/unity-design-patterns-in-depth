using UnityEngine;

namespace Prototype.Naive {
  public class ProceduralSphere : ProceduralShape {
    private float _radius;
    public int _horizontalSegments;
    public int _verticalSegments;

    public float Radius => _radius;
    public int HorizontalSegments => _horizontalSegments;
    public int VerticalSegments => _verticalSegments;

    public ProceduralSphere(string name, Color color, Vector3 position, float radius = 1f, int horizontalSegments = 16, int verticalSegments = 16)
    : base(name, color, position) {
      _radius = radius;
      _horizontalSegments = horizontalSegments;
      _verticalSegments = verticalSegments;
      CreateMesh();
      _gameObject.AddComponent<SphereCollider>();
    }

    protected override void CreateMeshData() {
      float horizontalSegmentAngle = 360f / _horizontalSegments;
      float verticalSegmentAngle = 180f / _verticalSegments;
      float currentVerticalAngle = -90;

      for (int y = 0; y <= _verticalSegments; y++) {
        float currentHorizontalAngle = 0f;
        for (int x = 0; x <= _horizontalSegments; x++) {
          Vector3 point = PointOnSpheroid(_radius, _radius, currentHorizontalAngle, currentVerticalAngle);
          vertices.Add(point);
          normals.Add(point.normalized);
          uv.Add(new Vector2((float)x / _horizontalSegments, (float)y / _verticalSegments));
          currentHorizontalAngle -= horizontalSegmentAngle;
        }
        currentVerticalAngle += verticalSegmentAngle;
      }

      // Extra vertices due to the uvmap seam
      int horizontalCount = _horizontalSegments + 1;
      for (int ring = 0; ring < _verticalSegments; ring++) {
        for (int i = 0; i < horizontalCount - 1; i++) {
          int i0 = ring * horizontalCount + i;
          int i1 = (ring + 1) * horizontalCount + i;
          int i2 = ring * horizontalCount + i + 1;
          int i3 = (ring + 1) * horizontalCount + i + 1;

          triangles.Add(i0);
          triangles.Add(i1);
          triangles.Add(i2);
          triangles.Add(i2);
          triangles.Add(i1);
          triangles.Add(i3);
        }
      }
    }

    private static Vector3 PointOnSpheroid(float radius, float height, float horizontalAngle, float verticalAngle) {
      float horizontalRadians = horizontalAngle * Mathf.Deg2Rad;
      float verticalRadians = verticalAngle * Mathf.Deg2Rad;
      float cosVertical = Mathf.Cos(verticalRadians);

      return new Vector3(
          x: radius * Mathf.Sin(horizontalRadians) * cosVertical,
          y: height * Mathf.Sin(verticalRadians),
          z: radius * Mathf.Cos(horizontalRadians) * cosVertical);
    }

    public override void OnUpdate() {
      // _shapeMonoBehaviour.transform.Rotate(Vector3.up * Time.deltaTime * 10f);
    }

  }
}
