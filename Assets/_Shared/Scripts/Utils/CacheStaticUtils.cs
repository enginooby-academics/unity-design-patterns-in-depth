using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// This utility is for quickly performing action on components w/o declaring component fields in MonoBehaviour and calling GetComponent(). <br/>
/// The GameObject w/ used components are cached. If component is changed in runtime, call ClearCache().
/// </summary>
public static class CacheStaticUtils {
  // ! Only include common actions
  // ? Use GameObject or MonoBehaviour
  // ? UnityEngine.Object (getter not implemented) or UnityEngine.Component
  private static Dictionary<GameObject, Dictionary<Type, UnityEngine.Object>> _cachedGameObjects = new Dictionary<GameObject, Dictionary<Type, UnityEngine.Object>>();
  private readonly static bool ENABLE_DEBUG = false;


#if UNITY_EDITOR
  // TIP: Clear static variable each time entering Play Mode w/ this attribute
  [UnityEditor.InitializeOnEnterPlayMode]
#endif
  public static void ClearCachedGameObjects() {
    _cachedGameObjects = new Dictionary<GameObject, Dictionary<Type, UnityEngine.Object>>();
  }

  // E.g. Cache implicit MeshRenderer: gameObject.SetMaterialColor(Color.red) 
  // (pros: clean/short code esp. if perform action frequently | cons: need to implement delegated action here)
  // vs.  Cache explicit MeshRenderer: gameObject.GetCache<MeshRenderer>().material.color = Color.red;
  // REFACTOR: duplicate w/ ProcessActionWithCaching
  /// <summary>
  /// Find and return component from caches. If not found, get component and add it to caches.
  /// </summary>
  public static T GetCache<T>(this GameObject go) where T : UnityEngine.Object {
    if (_cachedGameObjects.TryGetValue(go, out var components)) {
      if (components.TryGetValue(typeof(T), out var component)) {
        return (T)component;
      } else {
        return (T)CacheComponent(go, typeof(T), components);
      }
    }

    CacheGameObject(go, out components);
    return (T)CacheComponent(go, typeof(T), components);
  }

  /// <summary>
  /// [CacheStaticUtils] Clear all static caches of this MonoBehaviour. Use when a certain component is changed.
  /// </summary>
  public static void ClearCaches(this GameObject go) {
    _cachedGameObjects.Remove(go);
  }

  /// <summary>
  /// [CacheStaticUtils] Clear a component cache of this MonoBehaviour. Use when the component is changed.
  /// </summary>
  public static void ClearCache(this GameObject go, Type componentType) {
    if (_cachedGameObjects.TryGetValue(go, out var components)) {
      components[componentType] = null;
    }
  }

  // TODO: Commonly-used components such as RigidBody, AudioSource, Animator

  /// <summary>
  /// [CacheStaticUtils] Require static MeshRenderer component.
  /// </summary>
  public static void SetMaterialColor(this GameObject go, Color color, bool isMaterialShared = false) {
    void PerformAction(UnityEngine.Object component) {
      if ((component as MeshRenderer).material == null)
        (component as MeshRenderer).material = new Material(Shader.Find("Universal Render Pipeline/Lit"));

      if (isMaterialShared)
        (component as MeshRenderer).sharedMaterial.color = color;
      else
        (component as MeshRenderer).material.color = color;
    }

    ProcessActionWithCaching(go, PerformAction, typeof(MeshRenderer));
  }

  /// <summary>
  /// [CacheStaticUtils] Require static MeshFilter component. <br/>
  /// Convert the given enum to PrimitiveType, hence the string name must match. Useful for enum subset of PrimitiveType if don't want to include all primitive meshes. 
  /// </summary>
  public static void SetPrimitiveMesh(this GameObject go, Enum meshType) {
    void PerformAction(UnityEngine.Object component) {
      (component as MeshFilter).mesh = PrimitiveUtils.GetPrimitiveMesh(meshType);
    }

    ProcessActionWithCaching(go, PerformAction, typeof(MeshFilter));
  }

  /// <summary>
  /// [CacheStaticUtils] Require static TransformOperator component.
  /// </summary>
  public static void InvertTranslationalDirection(this GameObject go) {
    void PerformAction(UnityEngine.Object component) {
      (component as TransformOperator).InvertTranslationalDirection();
    }

    ProcessActionWithCaching(go, PerformAction, typeof(TransformOperator));
  }

  #region INTERNAL METHODS ===================================================================================================================================
  /// <summary>
  /// Cache GameObject and its component after performing component action.
  /// </summary>
  private static void ProcessActionWithCaching(GameObject go, Action<UnityEngine.Object> PerformAction, Type componentType) {
    if (_cachedGameObjects.TryGetValue(go, out var components)) { // O(1)
      if (components.TryGetValue(componentType, out var component)) { // O(1)
        PerformAction?.Invoke(component);
      } else CacheComponent(go, componentType, components, PerformAction);
      return;
    }
    CacheGameObject(go, out components);
    CacheComponent(go, componentType, components, PerformAction, false);
  }

  private static void CacheGameObject(GameObject go, out Dictionary<Type, UnityEngine.Object> components) {
    components = new Dictionary<Type, UnityEngine.Object>();
    _cachedGameObjects.Add(go, components);
  }

  private static UnityEngine.Object CacheComponent(GameObject go, Type componentType, Dictionary<Type, UnityEngine.Object> components, Action<UnityEngine.Object> PerformAction = null, bool isGameObjectCached = true) {
    if (go.TryGetComponent(componentType, out var component)) {
      if (ENABLE_DEBUG) Debug.Log($"{go.name} cached: {isGameObjectCached}. Caching {component.GetType()}");
      components.Add(componentType, component);
      PerformAction?.Invoke(component);
      return component;
    }

    // ? Which Exception
    // ? Create new component if not found
    throw new ArgumentException($"Component {componentType.Name} on {go.name} not found!");
  }
  #endregion INTERNAL METHODS ================================================================================================================================
}