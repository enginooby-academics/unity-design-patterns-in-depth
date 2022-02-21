using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class ProceduralUtils {
  private static readonly List<Vector3> vertices = new();
  private static readonly List<int> triangles = new();
  private static readonly List<Vector3> normals = new();
  private static readonly List<Vector4> tangents = new();
  private static readonly List<Vector2> uv = new();
  private static readonly List<Vector2> uv2 = new();
  private static readonly List<Vector2> uv3 = new();
  private static readonly List<Vector2> uv4 = new();

  private static void SetMeshData(Mesh mesh) {
    mesh.Clear();
    mesh.indexFormat = IndexFormat.UInt16;
    mesh.SetVertices(vertices);
    mesh.SetTriangles(triangles, 0, true);
    mesh.SetNormals(normals);
    mesh.SetTangents(tangents);
    mesh.SetUVs(0, uv);
    mesh.SetUVs(1, uv2);
    mesh.SetUVs(2, uv3);
    mesh.SetUVs(3, uv4);
  }

  private static void ClearMeshData() {
    vertices.Clear();
    triangles.Clear();
    normals.Clear();
    tangents.Clear();
    uv.Clear();
    uv2.Clear();
    uv3.Clear();
    uv4.Clear();
  }

  public static Mesh CreateCubeMesh(float size = 1f) {
    var mesh = new Mesh();
    ClearMeshData();
    CreateCubeMeshData(size);
    SetMeshData(mesh);
    return mesh;
  }

  private static void CreateCubeMeshData(float size = 1f) {
    var width = size * Vector3.right;
    var length = size * Vector3.forward;
    var height = size * Vector3.up;

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

  private static void AddQuadUV(
    Vector3 vertex0,
    Vector3 vertex1,
    Vector3 vertex2,
    Vector3 vertex3,
    Vector3 normal,
    Vector2 uv0,
    Vector2 uv1,
    Vector2 uv2,
    Vector2 uv3) {
    uv.Add(uv0);
    uv.Add(uv1);
    uv.Add(uv2);
    uv.Add(uv3);
    AddQuadNormal(vertex0, vertex1, vertex2, vertex3, normal, normal, normal, normal);
  }

  private static void AddQuadNormal(
    Vector3 vertex0,
    Vector3 vertex1,
    Vector3 vertex2,
    Vector3 vertex3,
    Vector3 normal0,
    Vector3 normal1,
    Vector3 normal2,
    Vector3 normal3) {
    normals.Add(normal0);
    normals.Add(normal1);
    normals.Add(normal2);
    normals.Add(normal3);
    AddQuad(vertex0, vertex1, vertex2, vertex3);
  }

  private static void AddQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3) {
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