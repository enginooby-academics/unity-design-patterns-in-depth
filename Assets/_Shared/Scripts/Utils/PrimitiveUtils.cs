using System;
using System.Collections.Generic;
using UnityEngine;

public static class PrimitiveUtils {
  private static Dictionary<PrimitiveType, Mesh> _meshes = new Dictionary<PrimitiveType, Mesh>();

  public static GameObject CreatePrimitive(PrimitiveType type, bool withCollider) {
    if (withCollider) { return GameObject.CreatePrimitive(type); }

    GameObject gameObject = new GameObject(type.ToString());
    gameObject.HideInHierarchy();
    MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
    meshFilter.sharedMesh = GetPrimitiveMesh(type);
    gameObject.AddComponent<MeshRenderer>();

    return gameObject;
  }

  public static Mesh GetPrimitiveMesh(PrimitiveType type) {
    if (!_meshes.ContainsKey(type)) {
      CreatePrimitiveMesh(type);
    }

    return _meshes[type];
  }

  /// <summary>
  /// Convert the given enum to PrimitiveType, hence the string name must match. Useful for enum subset of PrimitiveType if don't want to include all primitive meshes. 
  /// </summary>
  public static Mesh GetPrimitiveMesh(Enum type) {
    Enum.TryParse(type.ToString(), out PrimitiveType primitiveType);
    return GetPrimitiveMesh(primitiveType);
  }

  private static Mesh CreatePrimitiveMesh(PrimitiveType type) {
    GameObject gameObject = GameObject.CreatePrimitive(type);
    Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
    // just to normalize cylinder height as other shapes as cube, sphere...
    if (type == PrimitiveType.Cylinder) mesh = mesh.WithScale(.5f, AxisFlag.Y);
    GameObject.Destroy(gameObject);
    _meshes[type] = mesh;
    return mesh;
  }
}
