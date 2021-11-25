using UnityEngine;
using System.Collections.Generic;

public static class RayUtils {
  /// <summary>
  /// Ray casted from main camera to mouse position.
  /// </summary>
  public static Ray MouseRay => Camera.main.ScreenPointToRay(Input.mousePosition);

  /// <summary>
  /// If ray casted from main camera to mouse position hit anything.
  /// </summary>
  public static bool IsMouseRayHit => Physics.Raycast(MouseRay);

  /// <summary>
  /// Hits from the ray casted from main camera to mouse position.
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