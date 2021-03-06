using UnityEngine;

namespace Prototype {
  public class ProceduralCube : ProceduralShape {
    protected float _size;

    public ProceduralCube(string name, Color color, Vector3 position, float size = 1f)
      : base(name, color, position) {
      _size = size;
      CreateMesh();
      _gameObject.AddComponent<BoxCollider>();
    }

    protected override void CreateMeshData() {
      var width = _size * Vector3.right;
      var length = _size * Vector3.forward;
      var height = _size * Vector3.up;

      var v000 = -width / 2 - length / 2 - height / 2;
      var v001 = v000 + height;
      var v010 = v000 + width;
      var v011 = v000 + width + height;
      var v100 = v000 + length;
      var v101 = v000 + length + height;
      var v110 = v000 + width + length;
      var v111 = v000 + width + length + height;

      var uv0 = new Vector2(0, 0);
      var uv1 = new Vector2(0, 1);
      var uv2 = new Vector2(1, 1);
      var uv3 = new Vector2(1, 0);

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

    public override void OnUpdate() {
      // _shapeMonoBehaviour.transform.Rotate(Vector3.up * Time.deltaTime * 10f);
    }
  }
}