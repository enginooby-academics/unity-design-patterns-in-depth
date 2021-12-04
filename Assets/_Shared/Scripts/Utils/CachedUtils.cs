using System;
using UnityEngine;

/// <summary>
/// This utility is for quick modifying properties of static component (not changed in runtime) due to caching.
/// Use to replace all GetComponent() code.
/// </summary>
public static class CachedUtils {
  private static MonoBehaviour _cachedMonoBehaviour;
  private static Material _cachedMaterial;
  private static MeshFilter _cachedMeshFilter;

  // [InitializeOnEnterPlayMode]
  // private static void ResetCachesOnEnterPlayMode() {
  //   _cachedMonoBehaviour = null;
  //   _cachedMaterial = null;
  //   _cachedMeshFilter = null;
  // }

  private static void ResetCachesOnMonoBehaviourChanged() {
    _cachedMaterial = null;
    _cachedMeshFilter = null;
  }

  /// <summary>
  /// [CachedUtil] Required MeshRenderer component w/ static Material.
  /// </summary>
  public static void SetMaterialColor(this MonoBehaviour monoBehaviour, Color color) {
    // REFACTOR
    if (monoBehaviour == _cachedMonoBehaviour && _cachedMaterial) {
      // ? handle case where material is changed in runtime
      _cachedMaterial.color = color;
      return;
    }

    // Debug.Log("Getting Component MeshRenderer");
    if (monoBehaviour.TryGetComponent<MeshRenderer>(out var meshRenderer)) {
      meshRenderer.material.color = color;
      _cachedMonoBehaviour = monoBehaviour;
      ResetCachesOnMonoBehaviourChanged();
      _cachedMaterial = meshRenderer.material;
    } else {
      Debug.LogError($"Component MeshRenderer not found!");
    }
  }

  /// <summary>
  /// [CachedUtil] Required static MeshFilter component.
  /// Convert the given enum to PrimitiveType, hence the string name must match. Useful for enum subset of PrimitiveType if don't want to include all primitive meshes. 
  /// </summary>
  public static void SetPrimitiveMesh(this MonoBehaviour monoBehaviour, Enum type) {
    if (monoBehaviour == _cachedMonoBehaviour && _cachedMeshFilter) {
      _cachedMeshFilter.mesh = PrimitiveUtils.GetPrimitiveMesh(type);
      return;
    }

    // Debug.Log("Getting Component MeshFilter");
    if (monoBehaviour.TryGetComponent<MeshFilter>(out var meshFilter)) {
      meshFilter.mesh = PrimitiveUtils.GetPrimitiveMesh(type);
      _cachedMonoBehaviour = monoBehaviour;
      ResetCachesOnMonoBehaviourChanged();
      _cachedMeshFilter = meshFilter;
    } else {
      Debug.LogError($"Component MeshFilter not found!");
    }
  }

  // IMPL
  private static void DebugComponentNotFound(this Component component) {
    // Debug.LogError($"Component {typeof().ToString()} not found!");
  }
}