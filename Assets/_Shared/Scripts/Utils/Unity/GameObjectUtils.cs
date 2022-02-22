using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Enginooby.Utils;
using UnityEngine.SceneManagement;

public static class GameObjectUtils {
  // ? Rename to SetPosition, reserve With... for duplication operations
  public static GameObject WithPosition(this GameObject go, float x, float y, float z) {
    go.transform.position = new Vector3(x, y, z);
    return go;
  }

  public static void SetPosition(this GameObject go, Vector3 pos) => go.transform.position = pos;

  public static GameObject WithScale(this GameObject go, float x, float y, float z) {
    go.transform.localScale = new Vector3(x, y, z);
    return go;
  }

  /// <summary>
  ///   Set local rotation in degree unit.
  /// </summary>
  public static GameObject WithRotation(this GameObject go, float x, float y, float z) {
    go.transform.localEulerAngles = new Vector3(x, y, z);
    return go;
  }

  public static GameObject WithMaterial(this GameObject go, Color color) {
    go.SetMaterialColor(color);
    return go;
  }

  // ? activeSelf or activeInHierarchy
  public static void ToggleActive(this GameObject go) => go.SetActive(!go.activeSelf);

  public static void ToggleActive(this IEnumerable<GameObject> gos) => gos.ForEach(ToggleActive);

  /// <summary>
  ///   The GameObject is not shown in the Hierarchy, not saved to to Scenes, and not unloaded by
  ///   Resources.UnloadUnusedAssets().
  /// </summary>
  public static void HideAndDontSave(this GameObject go) => go.hideFlags = HideFlags.HideAndDontSave;

  /// <summary>
  ///   The object will not appear in the hierarchy.
  /// </summary>
  public static void HideInHierarchy(this GameObject go) => go.hideFlags = HideFlags.HideInHierarchy;

  /// <summary>
  ///   DestroyImmediate if in Edit Mode.
  /// </summary>
  public static void Destroy(this GameObject go) {
    if (Application.isPlaying)
      Object.Destroy(go);
    else
      Object.DestroyImmediate(go);
  }

  public static void Destroy(this GameObject go, bool @if) {
    if (@if) go.Destroy();
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
  ///   Create a new GameObject with the given component type and reset transform.
  ///   <returns>The added component</returns>
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
    var nearestGoIndex = 0;
    var currentMinDist = Mathf.Infinity;

    for (var i = 0; i < gos.Count(); i++) {
      var go = gos.ElementAt(i);
      var distToPos = go.transform.GetDistanceTo(pos);
      if (distToPos < currentMinDist) {
        currentMinDist = distToPos;
        nearestGoIndex = i;
      }
    }

    return gos.ElementAt(nearestGoIndex);
  }

  /// <summary>
  /// Get all GameObjects by name in the active scene.
  /// </summary>
  public static IEnumerable<GameObject> FindAllGameObjects(string name) {
    var gos = new List<GameObject>();
    var rootGos = SceneManager.GetActiveScene().GetRootGameObjects();
    // Use scene roots to iterate and check each GameObject.

    foreach (var rootGo in rootGos) {
      // If the name matches, add the root GameObject to the target list.
      if (rootGo.name == name) gos.Add(rootGo);
      // Get the number of children of the scene root GameObject.
      var childCount = rootGo.transform.childCount;

      for (var i = 0; i < childCount; i++) {
        var childGo = rootGo.transform.GetChild(i).gameObject;
        if (childGo.name == name) gos.Add(childGo);
      }
    }

    return gos;
  }

  public static IEnumerable<GameObject> GetChildGameObjects(this GameObject go) {
    var childGos = new List<GameObject>();

    for (var i = 0; i < go.transform.childCount; i++) {
      var childGo = go.transform.GetChild(i).gameObject;
      childGos.Add(childGo);
    }

    return childGos;
  }

  /// <summary>
  /// Return all GameObjects at the same level in the hierarchy, excluding itself.
  /// </summary>
  public static IEnumerable<GameObject> GetSiblingGameObjects(this GameObject go) {
    var siblings = go.transform.parent.gameObject.GetChildGameObjects();
    return siblings.Where(sibling => sibling != go);
  }
}