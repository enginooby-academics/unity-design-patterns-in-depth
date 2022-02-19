using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RayUtils {
  /// <summary>
  ///   Ray casted from main camera to mouse position.
  /// </summary>
  public static Ray MouseRay => Camera.main!.ScreenPointToRay(Input.mousePosition);

  /// <summary>
  ///   If ray casted from main camera to mouse position hit any collider.
  /// </summary>
  public static bool IsMouseRayHit => Physics.Raycast(MouseRay);

  /// <summary>
  ///   Hits from the ray casted from main camera to mouse position. GameObject needs Collider to be hit.
  /// </summary>
  public static RaycastHit[] HitsFromMouseRay => Physics.RaycastAll(MouseRay);

  public static Vector3? MousePosOnRayHit {
    get {
      if (Physics.Raycast(MouseRay, out var hit)) return hit.point;

      return null;
    }
  }

  /// <summary>
  ///   Return the list of GameObjects lie between this GameObject and main camera.
  /// </summary>
  // FIX: not working
  public static IEnumerable<GameObject> GetHitsFromCameraRay(this GameObject go) {
    var goToCameraRay = Camera.main!.ScreenPointToRay(go.transform.position);
    // RaycastHit[] hits = Physics.RaycastAll(goToCameraRay);
    var hits = Physics.RaycastAll(goToCameraRay);

    return hits.Select(hit => hit.transform.gameObject);
  }

  public static void DrawRayToCamera(this GameObject go) {
    var position = go.transform.position;
    var dirToCamera = Camera.main!.transform.position - position;
    Debug.DrawRay(position, dirToCamera, Color.yellow);
  }

  /// <summary>
  ///   Cast a ray from main camera to mouse position
  ///   then retrieve the list of all the components of the given type from Hits
  /// </summary>
  public static List<T> GetComponentsViaMouseRay<T>() {
    var targets = new List<T>();
    foreach (var hit in HitsFromMouseRay)
      if (hit.transform.TryGetComponent(out T target))
        targets.Add(target);

    return targets;
  }

  /// <summary>
  ///   Check if there is any GOs has the given component type is under cursor and store those component in the given list.
  /// </summary>
  public static bool IsMouseAtComponent<T>(out List<T> components) where T : Component {
    components = GetComponentsViaMouseRay<T>();
    return components.Count > 0;
  }

  public static bool IsMouseAtComponent<T>() where T : Component => GetComponentsViaMouseRay<T>().Count > 0;
}