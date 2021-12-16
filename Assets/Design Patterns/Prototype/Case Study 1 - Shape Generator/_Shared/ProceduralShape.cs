using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Prototype {
  public abstract class ProceduralShape : ICloneable {
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
    protected Color _color;

    public GameObject GameObject => _gameObject;
    public string Name => _gameObject.name;
    public Color Color => _color;
    public Vector3 Position {
      get => _gameObject.transform.position;
      set => _gameObject.transform.position = value;
    }

    public void SetPosition(Vector3 pos) {
      _gameObject.transform.position = pos;
    }

    // TODO: uv, Material, shader, behaviours (spin, shake) 

    protected ProceduralShape(string name, Color color, Vector3 position) {
      _gameObject = new GameObject(name);
      _gameObject.transform.position = position;

      _meshFilter = _gameObject.AddComponent<MeshFilter>();
      _meshRenderer = _gameObject.AddComponent<MeshRenderer>();
      _mesh = _meshFilter.mesh;

      var material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
      material.color = color;
      _meshRenderer.material = material;
      _color = color;

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

    protected abstract void CreateMeshData();
    public abstract void OnUpdate();

    // ! For base implementation
    public virtual object Clone(Vector3? pos) {
      throw new System.NotImplementedException();
    }
  }
}
