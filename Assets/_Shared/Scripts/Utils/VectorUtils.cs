using UnityEngine;

public static class VectorUtils {
  public static Vector3 v001 = Vector3.forward;
  public static Vector3 v011 = new Vector3(0, 1, 1);
  public static Vector3 v00m1 = Vector3.back;
  public static Vector3 v010 = Vector3.up;
  public static Vector3 v0m10 = Vector3.down;
  public static Vector3 vm100 = Vector3.left;
  public static Vector3 v100 = Vector3.right;

  /// <summary>Return true if Vector2.zero, used to treat unset Vector as infinite Vector (e.g. in Boundary)</summary>
  public static bool ContainsIgnoreZero(this Vector2 vect, float value) {
    return vect == Vector2.zero || vect.Contains(value);
  }

  /// <summary>
  /// Check contain inclusively.
  /// </summary>
  public static bool Contains(this Vector2 vect, float value) {
    return (vect.x <= value && value <= vect.y);
  }

  /// <summary> Uses a Vector2 where x is min and y is max. </summary>
  public static float Clamp(this Vector2 vect, float value) {
    return Mathf.Clamp(value, vect.x, vect.y);
  }

  public static float Average(this Vector2 vect) {
    return (vect.x + vect.y) / 2;
  }

  public static float Length(this Vector2 vect) {
    return Mathf.Abs(vect.x - vect.y);
  }

  public static float Random(this Vector2 vect) {
    return UnityEngine.Random.Range(vect.x, vect.y);
  }

  public static int Random(this Vector2Int vect) {
    return UnityEngine.Random.Range(vect.x, vect.y);
  }

  public static Vector2 Offset(this Vector2 vect, float offset) {
    return new Vector2(vect.x + offset, vect.y + offset);
  }

  public static Vector3 SetX(this Vector3 vect, float newX) {
    return new Vector3(newX, vect.y, vect.z);
  }

  public static Vector3 SetY(this Vector3 vect, float newY) {
    return new Vector3(vect.x, newY, vect.z);
  }

  public static Vector3 SetZ(this Vector3 vect, float newZ) {
    return new Vector3(vect.x, vect.y, newZ);
  }

  /// <summary>
  /// Return Vector3.zero if no flag.
  /// </summary>
  public static Vector3 ToVector3(this AxisFlag axisFlag) {
    if (axisFlag.HasFlag(AxisFlag.X) && axisFlag.HasFlag(AxisFlag.Y) && axisFlag.HasFlag(AxisFlag.Z)) return Vector3.one;
    if (!axisFlag.HasFlag(AxisFlag.X) && !axisFlag.HasFlag(AxisFlag.Y) && !axisFlag.HasFlag(AxisFlag.Z)) return Vector3.zero;
    if (!axisFlag.HasFlag(AxisFlag.X) && axisFlag.HasFlag(AxisFlag.Y) && axisFlag.HasFlag(AxisFlag.Z)) return v011;
    if (!axisFlag.HasFlag(AxisFlag.X) && !axisFlag.HasFlag(AxisFlag.Y) && axisFlag.HasFlag(AxisFlag.Z)) return v001;
    if (!axisFlag.HasFlag(AxisFlag.X) && axisFlag.HasFlag(AxisFlag.Y) && !axisFlag.HasFlag(AxisFlag.Z)) return v010;
    if (axisFlag.HasFlag(AxisFlag.X) && !axisFlag.HasFlag(AxisFlag.Y) && axisFlag.HasFlag(AxisFlag.Z)) return v010;
    if (axisFlag.HasFlag(AxisFlag.X) && !axisFlag.HasFlag(AxisFlag.Y) && !axisFlag.HasFlag(AxisFlag.Z)) return v011;
    if (axisFlag.HasFlag(AxisFlag.X) && axisFlag.HasFlag(AxisFlag.Y) && !axisFlag.HasFlag(AxisFlag.Z)) return v001;
    return Vector3.zero;
  }
}