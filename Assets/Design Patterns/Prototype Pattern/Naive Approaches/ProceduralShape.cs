using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype.Naive {
  public abstract class ProceduralShape {
    protected GameObject gameObject;
    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;
    protected Mesh mesh;
    protected Vector3[] vertices;
    protected int[] triangles;

    public GameObject GameObject => gameObject;

    // TODO: uv, Material, shader, behaviours (spin, shake) 

    protected ProceduralShape(string name, Color color, Vector3 position, Quaternion rotation, Vector3 localScale) {
      gameObject = new GameObject(name);
      gameObject.transform.position = position;
      gameObject.transform.rotation = rotation;
      gameObject.transform.localScale = localScale;
      
      meshFilter = gameObject.AddComponent<MeshFilter>();
      meshRenderer = gameObject.AddComponent<MeshRenderer>();
      mesh = meshFilter.mesh;

      var material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
      material.color = color;
      meshRenderer.material = material;

      var shapeMonoBehaviour = gameObject.AddComponent<ShapeMonoBehaviour>();
      shapeMonoBehaviour.shape = this;
    }

    protected void CreateMesh() {
      if (!mesh) return;

      CreateMeshData();
      mesh.Clear();
      mesh.vertices = vertices;
      mesh.triangles = triangles;
      mesh.Optimize();
      mesh.RecalculateNormals();
    }

    protected abstract void CreateMeshData();
    public abstract void OnUpdate();
  }

}
