using System;
using System.Collections.Generic;
using UnityEngine;

public static class PrimitiveUtils {
  private static readonly Dictionary<PrimitiveType, Mesh> _meshes = new();

  /// <summary>
  ///   Convert the given enum to PrimitiveType, hence the string name must match. Useful for enum subset of PrimitiveType if
  ///   don't want to include all primitive meshes.
  /// </summary>
  public static GameObject CreatePrimitive(Enum type, Color? color = null) {
    Enum.TryParse(type.ToString(), out PrimitiveType primitiveType);
    return CreatePrimitive(primitiveType, color);
  }

  public static GameObject CreatePrimitive(PrimitiveType type, Color? color = null) {
    var go = new GameObject(type.ToString());
    var meshFilter = go.AddComponent<MeshFilter>();
    var meshRenderer = go.AddComponent<MeshRenderer>();
    meshFilter.mesh = GetPrimitiveMesh(type);
    meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit")) {
      color = color ?? Color.white,
    };
    return go;
  }

  public static Mesh GetPrimitiveMesh(PrimitiveType type) {
    if (!_meshes.ContainsKey(type) ||
        !_meshes[type]) // non sharedMesh may be destroyed from the second times. In this case. also create new.
      CreatePrimitiveMesh(type);

    return _meshes[type];
  }


  /// <summary>
  ///   Create primitive GOs to retrieve primitive meshes.
  /// </summary>
  private static GameObject CreatePrimitive(PrimitiveType type, bool withCollider) {
    if (withCollider) return GameObject.CreatePrimitive(type);

    var gameObject = new GameObject(type.ToString());
    gameObject.HideInHierarchy();
    var meshFilter = gameObject.AddComponent<MeshFilter>();
    meshFilter.sharedMesh = GetPrimitiveMesh(type);
    gameObject.AddComponent<MeshRenderer>();

    return gameObject;
  }

  /// <summary>
  ///   Convert the given enum to PrimitiveType, hence the string name must match.
  ///   Useful for enum subset of PrimitiveType if don't want to include all primitive meshes.
  /// </summary>
  public static Mesh GetPrimitiveMesh(Enum type) {
    Enum.TryParse(type.ToString(), out PrimitiveType primitiveType);
    return GetPrimitiveMesh(primitiveType);
  }

  private static Mesh CreatePrimitiveMesh(PrimitiveType type) {
    var gameObject = GameObject.CreatePrimitive(type);
    var mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
    // just to normalize cylinder height as other shapes as cube, sphere...
    if (type == PrimitiveType.Cylinder) mesh = mesh.WithScale(.5f, AxisFlag.Y);
    gameObject.Destroy();
    _meshes[type] = mesh;
    return mesh;
  }
}