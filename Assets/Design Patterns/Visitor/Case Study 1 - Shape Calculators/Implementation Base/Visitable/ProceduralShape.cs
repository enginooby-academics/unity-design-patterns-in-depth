using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VisitorPattern.Case1.Base {
  public abstract class ProceduralShape : MonoBehaviour, ICalculatable {
    public float ProcessCalculation(ICalculator calculator) => calculator.Calculate(this);

    [Button]
    public void Calculate() {
      ICalculator calculator = CalculatorManager.Instance.CurrentCalculator;
      float result = ProcessCalculation(calculator);
      print($"{calculator.GetType().Name}: {result}");
    }

    #region PROCEDURAL-RELATED =======================================================================================================================================================================
    protected List<Vector3> vertices = new List<Vector3>();
    protected List<int> triangles = new List<int>();
    protected List<Vector3> normals = new List<Vector3>();
    protected List<Vector4> tangents = new List<Vector4>();
    protected List<Vector2> uv = new List<Vector2>();
    protected List<Vector2> uv2 = new List<Vector2>();
    protected List<Vector2> uv3 = new List<Vector2>();
    protected List<Vector2> uv4 = new List<Vector2>();

    protected MeshFilter _meshFilter;
    protected MeshRenderer _meshRenderer;

    private void Awake() {
      _meshFilter = gameObject.AddComponent<MeshFilter>();
      CreateMesh();
      _meshRenderer = gameObject.AddComponent<MeshRenderer>();
      _meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }

    // ? Extract to MeshUtils (link to Prototype Pattern as well)
    protected void CreateMesh() {
      if (!_meshFilter) return;

      ClearMeshData();
      CreateMeshData();
      Mesh _mesh = _meshFilter.mesh;
      _mesh.Clear();
      _mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;
      _mesh.SetVertices(vertices);
      _mesh.SetTriangles(triangles, 0, true);
      _mesh.SetNormals(normals);
      _mesh.SetTangents(tangents);
      _mesh.SetUVs(0, uv);
      _mesh.SetUVs(1, uv2);
      _mesh.SetUVs(2, uv3);
      _mesh.SetUVs(3, uv4);
    }

    private void ClearMeshData() {
      vertices.Clear();
      triangles.Clear();
      normals.Clear();
      tangents.Clear();
      uv.Clear();
      uv2.Clear();
      uv3.Clear();
      uv4.Clear();
    }

    protected virtual void CreateMeshData() { }
    #endregion PROCEDURAL-RELATED ====================================================================================================================================================================
  }
}
