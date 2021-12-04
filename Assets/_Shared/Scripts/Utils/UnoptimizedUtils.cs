using System;
using UnityEngine;
/// <summary>
/// This utility is for quick modifying component properties w/o caching the component.
/// Useful for prototyping or writing shot code.
/// For production, if modify frequently, cach the component instead.
/// </summary>
public static class UnoptimizedUtils {
  /// <summary>
  /// [Unoptimized] Required MeshRenderer component.
  /// </summary>
  public static void SetMaterialColor(this MonoBehaviour monoBehaviour, Color color) {
    if (monoBehaviour.TryGetComponent<MeshRenderer>(out var meshRenderer)) {
      meshRenderer.material.color = color;
    } else {
      Debug.LogError($"Component MeshRenderer not found!");
    }
  }

  /// <summary>
  /// [Unoptimized] Required MeshFilter component.
  /// Convert the given enum to PrimitiveType, hence the string name must match. Useful for enum subset of PrimitiveType if don't want to include all primitive meshes. 
  /// </summary>
  public static void SetPrimitiveMesh(this MonoBehaviour monoBehaviour, Enum type) {
    if (monoBehaviour.TryGetComponent<MeshFilter>(out var meshFilter)) {
      meshFilter.mesh = PrimitiveUtils.GetPrimitiveMesh(type);
    } else {
      Debug.LogError($"Component MeshFilter not found!");
    }
  }

  // IMPL
  private static void DebugComponentNotFound(this Component component) {
    // Debug.LogError($"Component {typeof().ToString()} not found!");
  }
}