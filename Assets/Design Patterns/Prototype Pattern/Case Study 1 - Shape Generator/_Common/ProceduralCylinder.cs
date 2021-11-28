using System.Collections.Generic;
using UnityEngine;

namespace Prototype {
  public class ProceduralCylinder : ProceduralShape {
    protected float _radius;
    protected int _segments;
    protected float _height;

    public ProceduralCylinder(string name, Color color, Vector3 position, float radius = 1f, int segments = 16, float height = 1f)
    : base(name, color, position) {
      _radius = radius;
      _segments = segments;
      _height = height;
      CreateMesh();
      MeshCollider collider = _gameObject.AddComponent<MeshCollider>();
      collider.convex = true;
    }

    protected override void CreateMeshData() {
      float segmentAngle = 360f / _segments;
      float currentAngle = 0;
      Vector3 halfHeightUp = Vector3.up * _height / 2;

      var lowerRing = new List<Vector3>(_segments);
      var lowerDiskUV = new List<Vector2>();
      var upperRing = new List<Vector3>(_segments);
      var upperDiskUV = new List<Vector2>();
      var strip = new List<Vector3>();
      var stripNormals = new List<Vector3>();
      var stripUV = new List<Vector2>();

      for (var i = 0; i < _segments; i++) {
        AddCylinderPoints(_radius, currentAngle, halfHeightUp, ref strip, ref stripUV, ref stripNormals, out Vector3 lowerVertex, out Vector3 upperVertex);
        lowerRing.Add(lowerVertex);
        upperRing.Add(upperVertex);
        Vector2 uv = PointOnCircle2(0.5f, currentAngle) + new Vector2(0.5f, 0.5f);
        upperDiskUV.Add(uv);
        uv.x = 1 - uv.x;
        lowerDiskUV.Add(uv);
        currentAngle += segmentAngle;
      }

      AddCylinderPoints(_radius, currentAngle, halfHeightUp, ref strip, ref stripUV, ref stripNormals, out _, out _);
      AddTriangleFan(lowerRing, Vector3.down, lowerDiskUV, true);
      AddTriangleFan(upperRing, Vector3.up, upperDiskUV);
      uv.AddRange(stripUV);
      AddTriangleStrip(strip, stripNormals);
    }

    private void AddCylinderPoints(float radius, float currentAngle, Vector3 halfHeightUp,
           ref List<Vector3> vertices, ref List<Vector2> uv, ref List<Vector3> normals,
           out Vector3 lowerVertex, out Vector3 upperVertex) {
      Vector3 normal = PointOnCircle3(0, 2, 1, currentAngle);
      Vector3 point = normal * radius;
      lowerVertex = point - halfHeightUp;
      upperVertex = point + halfHeightUp;

      vertices.Add(upperVertex);
      normals.Add(normal);
      vertices.Add(lowerVertex);
      normals.Add(normal);

      float u = 1 - currentAngle / 360;
      uv.Add(new Vector2(u, 1));
      uv.Add(new Vector2(u, 0));
    }

    private Vector3 PointOnCircle3(int xIndex, int yIndex, float radius, float angle) {
      float angleInRadians = angle * Mathf.Deg2Rad;
      var point = new Vector3();
      point[xIndex] = radius * Mathf.Sin(angleInRadians);
      point[yIndex] = radius * Mathf.Cos(angleInRadians);
      return point;
    }

    private Vector2 PointOnCircle2(float radius, float angle) {
      float angleInRadians = angle * Mathf.Deg2Rad;
      return new Vector2(radius * Mathf.Sin(angleInRadians), radius * Mathf.Cos(angleInRadians));
    }

    private void AddTriangleFan(IList<Vector3> fan, Vector3 normal, IList<Vector2> uv, bool reverseTriangles = false) {
      this.uv.AddRange(uv);
      AddTriangleFanVertices(fan, reverseTriangles);
      for (int i = 0; i < fan.Count; i++) {
        normals.Add(normal);
      }
    }

    private void AddTriangleStrip(IList<Vector3> strip, IList<Vector3> normals) {
      for (int i = 0, j = 1, k = 2;
          i < strip.Count - 2;
          i++, j += i % 2 * 2, k += (i + 1) % 2 * 2) {
        triangles.Add(i + vertices.Count);
        triangles.Add(j + vertices.Count);
        triangles.Add(k + vertices.Count);
      }
      vertices.AddRange(strip);
      this.normals.AddRange(normals);
    }

    private void AddTriangleFanVertices(IList<Vector3> fan, bool reverseTriangles) {
      int count = vertices.Count;
      if (reverseTriangles) {
        for (int i = fan.Count - 1; i > 1; i--) {
          triangles.Add(0 + count);
          triangles.Add(i + count);
          triangles.Add(i - 1 + count);
        }
      } else {
        for (int i = 1; i < fan.Count - 1; i++) {
          triangles.Add(0 + count);
          triangles.Add(i + count);
          triangles.Add(i + 1 + count);
        }
      }
      vertices.AddRange(fan);
    }

    public override void OnUpdate() {
      // _shapeMonoBehaviour.transform.Rotate(Vector3.up * Time.deltaTime * 10f);
    }

  }
}
