using Sirenix.OdinInspector;
using UnityEngine;

namespace VisitorPattern.Case1.Base2 {
  /// <summary>
  /// * A 'Concrete Visitable Element' class
  /// </summary>
  public class ProceduralCube : ProceduralShape {
    [SerializeField, OnValueChanged(nameof(CreateMesh)), Range(1f, 5f)]
    private float _size = 1f;
    public float Size => _size;

    protected override void CreateMeshData() {
      base.CreateMeshData();

      Vector3 width = _size * Vector3.right;
      Vector3 length = _size * Vector3.forward;
      Vector3 height = _size * Vector3.up;

      Vector3 v000 = -width / 2 - length / 2 - height / 2;
      Vector3 v001 = v000 + height;
      Vector3 v010 = v000 + width;
      Vector3 v011 = v000 + width + height;
      Vector3 v100 = v000 + length;
      Vector3 v101 = v000 + length + height;
      Vector3 v110 = v000 + width + length;
      Vector3 v111 = v000 + width + length + height;

      Vector2 uv0 = new Vector2(0, 0);
      Vector2 uv1 = new Vector2(0, 1);
      Vector2 uv2 = new Vector2(1, 1);
      Vector2 uv3 = new Vector2(1, 0);

      AddQuadUV(v100, v101, v001, v000, Vector3.left, uv0, uv1, uv2, uv3);
      AddQuadUV(v010, v011, v111, v110, Vector3.right, uv0, uv1, uv2, uv3);
      AddQuadUV(v010, v110, v100, v000, Vector3.down, uv0, uv1, uv2, uv3);
      AddQuadUV(v111, v011, v001, v101, Vector3.up, uv0, uv1, uv2, uv3);
      AddQuadUV(v000, v001, v011, v010, Vector3.back, uv0, uv1, uv2, uv3);
      AddQuadUV(v110, v111, v101, v100, Vector3.forward, uv0, uv1, uv2, uv3);
    }

    private void AddQuadUV(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 normal,
       Vector2 uv0, Vector2 uv1, Vector2 uv2, Vector2 uv3) {
      uv.Add(uv0);
      uv.Add(uv1);
      uv.Add(uv2);
      uv.Add(uv3);
      AddQuadNormal(vertex0, vertex1, vertex2, vertex3, normal, normal, normal, normal);
    }

    private void AddQuadNormal(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3,
            Vector3 normal0, Vector3 normal1, Vector3 normal2, Vector3 normal3) {
      normals.Add(normal0);
      normals.Add(normal1);
      normals.Add(normal2);
      normals.Add(normal3);
      AddQuad(vertex0, vertex1, vertex2, vertex3);
    }

    private void AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3) {
      triangles.Add(0 + vertices.Count);
      triangles.Add(1 + vertices.Count);
      triangles.Add(2 + vertices.Count);
      triangles.Add(0 + vertices.Count);
      triangles.Add(2 + vertices.Count);
      triangles.Add(3 + vertices.Count);
      vertices.Add(vertex0);
      vertices.Add(vertex1);
      vertices.Add(vertex2);
      vertices.Add(vertex3);
    }
  }
}
