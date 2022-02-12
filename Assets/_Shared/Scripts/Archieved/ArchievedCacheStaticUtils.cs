// using System.Collections.Generic;
// using System;
// using UnityEngine;

// // ? Create new component if not found
// /// <summary>
// /// This utility is for quickly performing action on components w/o declaring component fields in MonoBehaviour and calling GetComponent(). <br/>
// /// The GameObject w/ used components are cached. If component is changed in runtime, call ClearCache().
// /// </summary>
// public static class CacheStaticUtils {
//   // ! Only include common actions
//   // ? Use Object[] or List<Object> or Dictionary<string/Type type, Object> (dynamic and don't need explicit ids)
//   // ? Use GameObject or MonoBehaviour
//   private static Dictionary<GameObject, UnityEngine.Object[]> _cachedGameObjects = new Dictionary<GameObject, UnityEngine.Object[]>();
//   private readonly static bool ENABLE_DEBUG = true;
//   // ? Use enum instead
//   // ! Explicit cached object ids for fast lookup O(1) vs. find matched object type
//   public readonly static int MESH_RENDERRER_ID = 0;
//   public readonly static int MESH_FILTER_ID = 1;
//   public readonly static int TRANSFORM_OP_ID = 2;
//   public readonly static int COMPONENT_COUNT = 3; // equals to the last id + 1
//   // TODO: Commonly-used components such as RigidBody, AudioSource, Animator

//   // IMPL: cache by explicit Object type so that don't need to delegate the action
//   // E.g. Cache implicit MeshRenderer: gameObject.SetMaterialColor(Color.red) 
//   // (pros: clean/short code esp. if perform action frequently | cons: need to implement delegated action here and remember to use it)
//   // vs.  Cache explicit MeshRenderer: gameObject.GetCache<MeshRenderer>().material.color = Color.red;
//   // (cons: cannot constraint Object types which support caching with array ids)
//   // ? Add a _explicitCachedGameObjects fie
//   public static T GetCache<T>(this GameObject go) where T : UnityEngine.Object {
//     throw new NotImplementedException();
//   }

//   /// <summary>
//   /// [CacheStaticUtils] Clear all static caches of this MonoBehaviour. Use when a certain component is changed.
//   /// </summary>
//   public static void ClearCaches(this GameObject go) {
//     _cachedGameObjects.Remove(go);
//   }

//   /// <summary>
//   /// [CacheStaticUtils] Clear a component cache of this MonoBehaviour. Use when the component is changed.
//   /// </summary>
//   public static void ClearCache(this GameObject go, int componentId) {
//     if (_cachedGameObjects.TryGetValue(go, out var components)) {
//       components[componentId] = null;
//     }
//   }

//   /// <summary>
//   /// [CacheStaticUtils] Require static MeshRenderer component.
//   /// </summary>
//   public static void SetMaterialColor(this GameObject go, Color color, bool isMaterialShared = false) {
//     void PerformAction(UnityEngine.Object component) {
//       if (isMaterialShared) (component as MeshRenderer).sharedMaterial.color = color;
//       else (component as MeshRenderer).material.color = color;
//     }

//     ProcessActionWithCaching(go, PerformAction, MESH_RENDERRER_ID, typeof(MeshRenderer));
//   }

//   /// <summary>
//   /// [CacheStaticUtils] Require static MeshFilter component. <br/>
//   /// Convert the given enum to PrimitiveType, hence the string name must match. Useful for enum subset of PrimitiveType if don't want to include all primitive meshes. 
//   /// </summary>
//   public static void SetPrimitiveMesh(this GameObject go, Enum meshType) {
//     void PerformAction(UnityEngine.Object component) {
//       (component as MeshFilter).mesh = PrimitiveUtils.GetPrimitiveMesh(meshType);
//     }

//     ProcessActionWithCaching(go, PerformAction, MESH_FILTER_ID, typeof(MeshFilter));
//   }

//   /// <summary>
//   /// [CacheStaticUtils] Require static TransformOperator component.
//   /// </summary>
//   public static void InvertTranslationalDirection(this GameObject go) {
//     void PerformAction(UnityEngine.Object component) {
//       (component as TransformOperator).InvertTranslationalDirection();
//     }

//     ProcessActionWithCaching(go, PerformAction, TRANSFORM_OP_ID, typeof(TransformOperator));
//   }

//   #region INTERNAL METHODS ===================================================================================================================================
//   /// <summary>
//   /// Cache the GameObject and the component after performing action with it.
//   /// </summary>
//   private static void ProcessActionWithCaching(GameObject go, Action<UnityEngine.Object> PerformAction, int componentId, Type componentType) {
//     if (_cachedGameObjects.TryGetValue(go, out var components)) { // O(1)
//       if (components.TryGetById(componentId, out var component)) { // O(1)
//         PerformAction(component);
//       } else CacheComponent(go, PerformAction, componentId, componentType, components);
//       return;
//     }
//     CacheGameObject(go, out components);
//     CacheComponent(go, PerformAction, componentId, componentType, components, false);
//   }

//   private static void CacheGameObject(GameObject go, out UnityEngine.Object[] components) {
//     components = new UnityEngine.Object[COMPONENT_COUNT];
//     _cachedGameObjects.Add(go, components);
//   }

//   private static void CacheComponent(GameObject go, Action<UnityEngine.Object> PerformAction, int componentId, Type componentType, UnityEngine.Object[] components, bool isGameObjectCached = true) {
//     if (go.TryGetComponent(componentType, out var component)) {
//       if (ENABLE_DEBUG) Debug.Log($"GameObject cached: {isGameObjectCached}. Getting {component.GetType()}");
//       components[componentId] = component;
//       PerformAction(component);
//       return;
//     }
//     DebugComponentNotFound(componentType, go);
//   }

//   private static void DebugComponentNotFound(Type type, GameObject go) {
//     Debug.LogError($"Component {type.ToString()} on {go.name} not found!");
//   }
//   #endregion INTERNAL METHODS ================================================================================================================================
// }

