using UnityEngine;
using System.Collections.Generic;

public static class GameObjectUtils {
  public static void ToggleActive(this GameObject go) {
    go.SetActive(!go.activeInHierarchy); // ? activeSelf or activeInHierarchy
  }

  public static void ToggleActive(this List<GameObject> goList) {
    goList.ForEach(ToggleActive);
  }

  /// <summary>
  /// The GameObject is not shown in the Hierarchy, not saved to to Scenes, and not unloaded by Resources.UnloadUnusedAssets().
  /// </summary>
  public static void HideAndDontSave(this GameObject gameObject) {
    gameObject.hideFlags = HideFlags.HideAndDontSave;
  }

  /// <summary>
  /// The object will not appear in the hierarchy.
  /// </summary>
  public static void HideInHierarchy(this GameObject gameObject) {
    gameObject.hideFlags = HideFlags.HideInHierarchy;
  }

  /// <summary>
  /// DestroyImmediate if in Edit Mode.
  /// </summary>
  public static void Destroy(this GameObject gameObject) {
    if (Application.isPlaying) {
      Object.Destroy(gameObject);
    } else {
      Object.DestroyImmediate(gameObject);
    }
  }

  public static T TryAddComponent<T>(this GameObject go) where T : Component {
    if (go.TryGetComponent<T>(out T component)) return component;
    return go.AddComponent<T>();
  }
}