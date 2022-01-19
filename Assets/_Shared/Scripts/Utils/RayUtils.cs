using UnityEngine;
using System.Collections.Generic;

public static class RayUtils {
  /// <summary>
  /// Ray casted from main camera to mouse position.
  /// </summary>
  public static Ray MouseRay => Camera.main.ScreenPointToRay(Input.mousePosition);

  /// <summary>
  /// If ray casted from main camera to mouse position hit any collider.
  /// </summary>
  public static bool IsMouseRayHit => Physics.Raycast(MouseRay);

  /// <summary>
  /// Return the list of GameObjects lie between this GameObject and main camera.
  /// </summary>
  // FIX: not working
  public static List<GameObject> GetHitsFromCameraRay(this GameObject go) {
    var gos = new List<GameObject>();
    Ray goToCameraRay = Camera.main.ScreenPointToRay(go.transform.position);
    // RaycastHit[] hits = Physics.RaycastAll(goToCameraRay);
    RaycastHit[] hits = Physics.RaycastAll(goToCameraRay);

    foreach (var hit in hits) {
      gos.Add(hit.transform.gameObject);
    }

    return gos;
  }

  public static void DrawRayToCamera(this GameObject go) {
    var dirTocamera = Camera.main.transform.position - go.transform.position;
    Debug.DrawRay(go.transform.position, dirTocamera, Color.yellow);
  }

  /// <summary>
  /// Hits from the ray casted from main camera to mouse position. GameObject needs Collider to be hit.
  /// </summary>
  public static RaycastHit[] HitsFromMouseRay => Physics.RaycastAll(MouseRay);

  public static Vector3? MousePosOnRayHit {
    get {
      if (Physics.Raycast(MouseRay, out var hit)) {
        return hit.point;
      }

      return null;
    }
  }

  /// <summary>
  /// Cast a ray from main camera to mouse position 
  /// then retrieve the list of all the components of the given type from Hits
  /// </summary>
  public static List<T> GetComponentsViaMouseRay<T>() {
    var targets = new List<T>();
    foreach (RaycastHit hit in HitsFromMouseRay) {
      if (hit.transform.TryGetComponent<T>(out T target)) {
        targets.Add(target);
      }
    }

    return targets;
  }

  /// <summary>
  /// Check if there is any GOs has the given component type is under cursor and store those component in the given list.
  /// </summary>
  public static bool IsMouseAtComponent<T>(out List<T> components) where T : Component {
    components = GetComponentsViaMouseRay<T>();
    return components.Count > 0;
  }

  public static bool IsMouseAtComponent<T>() where T : Component {
    return GetComponentsViaMouseRay<T>().Count > 0;
  }
}