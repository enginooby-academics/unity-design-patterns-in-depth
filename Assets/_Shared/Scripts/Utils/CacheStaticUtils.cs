using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// This utility is for quick modifying properties of components. <br/>
/// The GameObject w/ modified components are cached. If component is changed in runtime, call ClearCache().
/// Used to remove all GetComponent() invokings.
/// </summary>
public static class CacheStaticUtils {
  // * Multi-cache
  // ? Use Object[] or List<Object>
  // ? Use GameObject or MonoBehaviour
  private static Dictionary<GameObject, UnityEngine.Object[]> _cachedGameObjects = new Dictionary<GameObject, UnityEngine.Object[]>();
  private readonly static bool ENABLE_DEBUG = false;
  public readonly static int MATERIAL_ID = 0;
  public readonly static int MESHFILTER_ID = 1;
  public readonly static int COMPONENT_COUNT = 2; // equals to last id + 1
  // TODO: Commonly-used components such as RigidBody, AudioSource, Animator

  /// <summary>
  /// [CacheStaticUtils] Clear all static caches of this MonoBehaviour. Use when a certain component is changed.
  /// </summary>
  public static void ClearCaches(this GameObject go) {
    _cachedGameObjects.Remove(go);
  }

  /// <summary>
  /// [CacheStaticUtils] Clear a component cache of this MonoBehaviour. Use when the component is changed.
  /// </summary>
  public static void ClearCache(this GameObject go, int componentId) {
    if (_cachedGameObjects.TryGetValue(go, out var components)) {
      components[componentId] = null;
    }
  }

  /// <summary>
  /// [CacheStaticUtils] Require static MeshRenderer component w/ static Material.
  /// </summary>
  // REFACTOR
  public static void SetMaterialColor(this GameObject go, Color color) {
    // GameObject cached
    if (_cachedGameObjects.TryGetValue(go, out var components)) {
      // component cached
      if (components.TryGetById<Material>(MATERIAL_ID, out Material cachedMaterial)) {
        cachedMaterial.color = color;
      } else {
        // component uncached
        if (ENABLE_DEBUG) Debug.Log("Cached GameObject: Getting MeshRenderer");
        if (go.TryGetComponent<MeshRenderer>(out var meshRenderer1)) {
          meshRenderer1.material.color = color;
          components[MATERIAL_ID] = meshRenderer1.material;
          return;
        }
        DebugComponentNotFound(typeof(MeshRenderer), go);
      }
      return;
    }

    // GameObject uncached
    if (ENABLE_DEBUG) Debug.Log("Un-cached GameObject: Getting MeshRenderer");
    if (go.TryGetComponent<MeshRenderer>(out var meshRenderer)) {
      meshRenderer.material.color = color;
      var newComponents = new UnityEngine.Object[COMPONENT_COUNT];
      newComponents[MATERIAL_ID] = meshRenderer.material;
      _cachedGameObjects.Add(go, newComponents);
      return;
    }
    DebugComponentNotFound(typeof(MeshRenderer), go);
  }

  /// <summary>
  /// [CacheStaticUtils] Require static MeshFilter component. <br/>
  /// Convert the given enum to PrimitiveType, hence the string name must match. Useful for enum subset of PrimitiveType if don't want to include all primitive meshes. 
  /// </summary>
  public static void SetPrimitiveMesh(this GameObject go, Enum type) {
    if (_cachedGameObjects.TryGetValue(go, out var components)) {
      if (components.TryGetById<MeshFilter>(MESHFILTER_ID, out var cachedMeshFilter)) {
        cachedMeshFilter.mesh = PrimitiveUtils.GetPrimitiveMesh(type);
        return;
      } else {
        if (ENABLE_DEBUG) Debug.Log("Cached GameObject: Getting MeshFilter");
        if (go.TryGetComponent<MeshFilter>(out var meshFilter1)) {
          meshFilter1.mesh = PrimitiveUtils.GetPrimitiveMesh(type);
          components[MESHFILTER_ID] = meshFilter1;
          return;
        }
        DebugComponentNotFound(typeof(MeshFilter), go);
      }
      return;
    }

    if (ENABLE_DEBUG) Debug.Log("Un-cached GameObject: Getting MeshFilter");
    if (go.TryGetComponent<MeshFilter>(out var meshFilter)) {
      meshFilter.mesh = PrimitiveUtils.GetPrimitiveMesh(type);
      var newComponents = new UnityEngine.Object[COMPONENT_COUNT];
      newComponents[MESHFILTER_ID] = meshFilter;
      _cachedGameObjects.Add(go, newComponents);
      return;
    }
    DebugComponentNotFound(typeof(MeshFilter), go);
  }

  private static void DebugComponentNotFound(Type type, GameObject go) {
    Debug.LogError($"Component {type.ToString()} on {go.name} not found!");
  }
}