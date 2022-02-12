using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class GizmosUtils {
  /// <summary>
  ///   Commonly used to draw speed: DrawGizmosLine(speed). Default color is magenta.
  /// </summary>
  public static void DrawGizmosDirection(this GameObject go, Vector3 offset, Color? color = null,
    float thickness = 2f) {
#if UNITY_EDITOR
    // TODO: draw decorative arrow & length number
    color ??= Color.magenta;
    Handles.color = color.Value;
    Handles.DrawLine(go.transform.position, go.transform.position + offset, thickness);
#endif
  }

  /// <summary>
  ///   Draw a boundary line from given position on the given axis.
  /// </summary>
  public static void DrawGizmozRangeFromPos(Vector3 pos, Vector2 range, Axis axisRange, Color? color = null,
    float thickness = 2f) {
#if UNITY_EDITOR
    Handles.color = color is null ? Color.magenta : color.Value;
    var lowerBound = new Vector3();
    var higherBound = new Vector3();

    switch (axisRange) {
      case Axis.X:
        lowerBound = new Vector3(range.x, pos.y, pos.z);
        higherBound = new Vector3(range.y, pos.y, pos.z);
        break;
      case Axis.Y:
        lowerBound = new Vector3(pos.x, range.x, pos.z);
        higherBound = new Vector3(pos.x, range.y, pos.z);
        break;
      case Axis.Z:
        lowerBound = new Vector3(pos.x, pos.y, range.x);
        higherBound = new Vector3(pos.x, pos.y, range.y);
        break;
    }

    Handles.DrawLine(lowerBound, higherBound, thickness);
#endif
  }

  /// <summary>
  ///   Draw a boundary line from gameobject position on the given axis.
  /// </summary>
  public static void DrawGizmozRangeFromPos(this MonoBehaviour monoBehaviour, Vector2 range, Axis axisRange,
    Color? color = null, float thickness = 2f) {
    DrawGizmozRangeFromPos(monoBehaviour.transform.position, range, axisRange, color);
  }
}