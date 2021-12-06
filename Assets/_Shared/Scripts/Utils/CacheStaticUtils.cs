using System.Collections.Generic;
using System;
using UnityEngine;

// ? Create new component if not found
/// <summary>
/// This utility is for quickly performing action on components w/o declaring component fields in MonoBehaviour and calling GetComponent(). <br/>
/// The GameObject w/ used components are cached. If component is changed in runtime, call ClearCache().
/// </summary>
public static class CacheStaticUtils {
  // ! Only include common actions
  // ? Use Object[] or List<Object>
  // ? Use GameObject or MonoBehaviour
  private static Dictionary<GameObject, UnityEngine.Object[]> _cachedGameObjects = new Dictionary<GameObject, UnityEngine.Object[]>();
  private readonly static bool ENABLE_DEBUG = true;
  public readonly static int MESH_RENDERRER_ID = 0;
  public readonly static int MESH_FILTER_ID = 1;
  public readonly static int TRANSFORM_OP_ID = 2;
  public readonly static int COMPONENT_COUNT = 3; // equals to last id + 1
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
  /// [CacheStaticUtils] Require static MeshRenderer component.
  /// </summary>
  public static void SetMaterialColor(this GameObject go, Color color, bool isMaterialShared = false) {
    void PerformAction(UnityEngine.Object component) {
      if (isMaterialShared) (component as MeshRenderer).sharedMaterial.color = color;
      else (component as MeshRenderer).material.color = color;
    }

    ProcessCaching(go, PerformAction, MESH_RENDERRER_ID, typeof(MeshRenderer));
  }

  /// <summary>
  /// [CacheStaticUtils] Require static MeshFilter component. <br/>
  /// Convert the given enum to PrimitiveType, hence the string name must match. Useful for enum subset of PrimitiveType if don't want to include all primitive meshes. 
  /// </summary>
  public static void SetPrimitiveMesh(this GameObject go, Enum meshType) {
    void PerformAction(UnityEngine.Object component) {
      (component as MeshFilter).mesh = PrimitiveUtils.GetPrimitiveMesh(meshType);
    }

    ProcessCaching(go, PerformAction, MESH_FILTER_ID, typeof(MeshFilter));
  }

  /// <summary>
  /// [CacheStaticUtils] Require static TransformOperator component.
  /// </summary>
  public static void InvertTranslationalDirection(this GameObject go) {
    void PerformAction(UnityEngine.Object component) {
      (component as TransformOperator).InvertTranslationalDirection();
    }

    ProcessCaching(go, PerformAction, TRANSFORM_OP_ID, typeof(TransformOperator));
  }

  #region INTERNAL METHODS ===================================================================================================================================
  private static void ProcessCaching(GameObject go, Action<UnityEngine.Object> PerformAction, int componentId, Type componentType) {
    if (_cachedGameObjects.TryGetValue(go, out var components)) {
      if (components.TryGetById(componentId, out var component)) {
        PerformAction(component);
      } else CacheComponent(go, PerformAction, componentId, componentType, components);
      return;
    }
    CacheGameObject(go, out components);
    CacheComponent(go, PerformAction, componentId, componentType, components, false);
  }

  private static void CacheGameObject(GameObject go, out UnityEngine.Object[] components) {
    components = new UnityEngine.Object[COMPONENT_COUNT];
    _cachedGameObjects.Add(go, components);
  }

  private static void CacheComponent(GameObject go, Action<UnityEngine.Object> PerformAction, int componentId, Type componentType, UnityEngine.Object[] components, bool isGameObjectCached = true) {
    if (go.TryGetComponent(componentType, out var component)) {
      if (ENABLE_DEBUG) Debug.Log($"GameObject cached: {isGameObjectCached}. Getting {component.GetType()}");
      components[componentId] = component;
      PerformAction(component);
      return;
    }
    DebugComponentNotFound(componentType, go);
  }

  private static void DebugComponentNotFound(Type type, GameObject go) {
    Debug.LogError($"Component {type.ToString()} on {go.name} not found!");
  }
  #endregion INTERNAL METHODS ================================================================================================================================
}