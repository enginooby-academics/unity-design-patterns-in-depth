using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public static class GameObjectUtils {
  // ? Rename to SetPosition, reserve With... for duplication operations
  public static GameObject WithPosition(this GameObject go, float x, float y, float z) {
    go.transform.position = new Vector3(x, y, z);
    return go;
  }

  public static void SetPosition(this GameObject go, Vector3 value) {
    go.transform.position = value;
  }

  public static GameObject WithScale(this GameObject go, float x, float y, float z) {
    go.transform.localScale = new Vector3(x, y, z);
    return go;
  }

  /// <summary>
  ///   Set local rotation in degree.
  /// </summary>
  public static GameObject WithRotation(this GameObject go, float x, float y, float z) {
    go.transform.localEulerAngles = new Vector3(x, y, z);
    return go;
  }

  public static GameObject WithMaterial(this GameObject go, Color color) {
    go.SetMaterialColor(color);
    return go;
  }

  public static void ToggleActive(this GameObject go) {
    // go.SetActive(!go.activeInHierarchy); // ? activeSelf or activeInHierarchy
    go.SetActive(!go.activeSelf); // ? activeSelf or activeInHierarchy
  }

  public static void ToggleActive(this List<GameObject> goList) {
    goList.ForEach(ToggleActive);
  }

  /// <summary>
  ///   The GameObject is not shown in the Hierarchy, not saved to to Scenes, and not unloaded by
  ///   Resources.UnloadUnusedAssets().
  /// </summary>
  public static void HideAndDontSave(this GameObject gameObject) {
    gameObject.hideFlags = HideFlags.HideAndDontSave;
  }

  /// <summary>
  ///   The object will not appear in the hierarchy.
  /// </summary>
  public static void HideInHierarchy(this GameObject gameObject) {
    gameObject.hideFlags = HideFlags.HideInHierarchy;
  }

  /// <summary>
  ///   DestroyImmediate if in Edit Mode.
  /// </summary>
  public static void Destroy(this GameObject gameObject) {
    if (Application.isPlaying)
      Object.Destroy(gameObject);
    else
      Object.DestroyImmediate(gameObject);
  }

  public static void Destroy(this GameObject gameObject, bool @if) {
    if (@if) gameObject.Destroy();
  }

  /// <summary>
  ///   Add component if not exist.
  /// </summary>
  public static T TryAddComponent<T>(this GameObject go) where T : Component =>
    go.TryGetComponent<T>(out var component) ? component : go.AddComponent<T>();

  /// <summary>
  ///   Create a new GameObject with given MonoBehaviour types with reset transform.
  /// </summary>
  public static void CreateGameObject(params Type[] componentTypes) {
    var go = new GameObject();
    go.transform.Reset();

    foreach (var monoBehaviourType in componentTypes) {
      if (!monoBehaviourType.IsSubclassOf<Component>()) return;
      go.AddComponent(monoBehaviourType);
    }
  }

  /// <summary>
  ///   Create a new GameObject with the given component type with reset transform.
  ///   Return the added MonoBehaviour.
  /// </summary>
  public static T CreateGameObject<T>() where T : Component {
    var go = new GameObject();
    go.transform.Reset();
    return go.AddComponent<T>();
  }

  /// <summary>
  ///   Return the GameObject in the list which is nearest to the given position.
  /// </summary>
  public static GameObject GetNearestTo(this IEnumerable<GameObject> gos, Vector3 pos) {
    var nearestIndex = 0;
    var lastDist = Mathf.Infinity;

    for (var i = 0; i < gos.Count(); i++) {
      var gameObject = gos.ElementAt(i);
      var distanceToPos = Vector3.Distance(pos, gameObject.transform.position);
      if (distanceToPos < lastDist) {
        nearestIndex = i;
        lastDist = distanceToPos;
      }
    }

    return gos.ElementAt(nearestIndex);
  }
}