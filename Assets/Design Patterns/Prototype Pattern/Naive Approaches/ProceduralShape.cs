using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Prototype.Naive {
  public abstract class ProceduralShape {
    protected List<Vector3> vertices = new List<Vector3>();
    protected List<int> triangles = new List<int>();
    protected List<Vector3> normals = new List<Vector3>();
    protected List<Vector4> tangents = new List<Vector4>();
    protected List<Vector2> uv = new List<Vector2>();
    protected List<Vector2> uv2 = new List<Vector2>();
    protected List<Vector2> uv3 = new List<Vector2>();
    protected List<Vector2> uv4 = new List<Vector2>();

    protected GameObject _gameObject;
    protected ShapeMonoBehaviour _shapeMonoBehaviour;
    protected MeshFilter _meshFilter;
    protected MeshRenderer _meshRenderer;
    protected Mesh _mesh;

    public GameObject GameObject => _gameObject;
    public string Name => _gameObject.name;
    public Vector3 Position => _gameObject.transform.position;
    public Quaternion Rottion => _gameObject.transform.rotation;
    public Vector3 LocalScale => _gameObject.transform.localScale;

    // TODO: uv, Material, shader, behaviours (spin, shake) 

    protected ProceduralShape(string name, Color color, Vector3 position, Quaternion rotation, Vector3 localScale) {
      _gameObject = new GameObject(name);
      _gameObject.transform.position = position;
      _gameObject.transform.rotation = rotation;
      _gameObject.transform.localScale = localScale;

      _meshFilter = _gameObject.AddComponent<MeshFilter>();
      _meshRenderer = _gameObject.AddComponent<MeshRenderer>();
      _mesh = _meshFilter.mesh;

      var material = new Material(Shader.Find("Standard"));
      material.color = color;
      _meshRenderer.material = material;

      _shapeMonoBehaviour = _gameObject.AddComponent<ShapeMonoBehaviour>();
      _shapeMonoBehaviour.shape = this;
      _shapeMonoBehaviour.AddRigidBodyIfNotExist(useGravity: true);
    }

    protected void CreateMesh() {
      if (!_mesh) return;

      CreateMeshData();
      _mesh.indexFormat = IndexFormat.UInt16;
      _mesh.name = Name;
      _mesh.SetVertices(vertices);
      _mesh.SetTriangles(triangles, 0, true);
      _mesh.SetNormals(normals);
      _mesh.SetTangents(tangents);
      _mesh.SetUVs(0, uv);
      _mesh.SetUVs(1, uv2);
      _mesh.SetUVs(2, uv3);
      _mesh.SetUVs(3, uv4);
    }

    protected void AddQuadUV(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 normal,
            Vector2 uv0, Vector2 uv1, Vector2 uv2, Vector2 uv3) {
      uv.Add(uv0);
      uv.Add(uv1);
      uv.Add(uv2);
      uv.Add(uv3);
      AddQuadNormal(vertex0, vertex1, vertex2, vertex3, normal, normal, normal, normal);
    }

    protected void AddQuadNormal(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3,
            Vector3 normal0, Vector3 normal1, Vector3 normal2, Vector3 normal3) {
      normals.Add(normal0);
      normals.Add(normal1);
      normals.Add(normal2);
      normals.Add(normal3);
      AddQuad(vertex0, vertex1, vertex2, vertex3);
    }

    protected void AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3) {
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

    protected abstract void CreateMeshData();
    public abstract void OnUpdate();
  }

}
