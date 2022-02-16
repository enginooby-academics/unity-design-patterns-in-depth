using UnityEngine;

public static class VectorUtils {
  // TODO: Rename to SNAKE_CASE and use Lazy
  public static readonly Vector3 v000 = Vector3.zero;

  /// <summary>
  ///   Vector3.zero
  /// </summary>
  public static readonly Vector3 v0 = Vector3.zero;

  public static readonly Vector3 v111 = Vector3.one;

  /// <summary>
  ///   Vector3.one
  /// </summary>
  public static readonly Vector3 v1 = Vector3.one;

  public static readonly Vector3 v001 = Vector3.forward;
  public static readonly Vector3 v011 = new Vector3(0, 1, 1);
  public static readonly Vector3 v101 = new Vector3(1, 0, 1);
  public static readonly Vector3 v110 = new Vector3(1, 1, 0);
  public static readonly Vector3 vm10m1 = new Vector3(-1, 0, -1);
  public static readonly Vector3 vm101 = new Vector3(-1, 0, 1);

  /// <summary>
  ///   Vector3(-1, 1, 0)
  /// </summary>
  public static readonly Vector3 vm110 = new Vector3(-1, 1, 0);

  /// <summary>
  ///   Vector3(1, -1, 0)
  /// </summary>
  public static readonly Vector3 v1m10 = new Vector3(1, -1, 0);

  /// <summary>
  ///   Vector3(-1, -1, 0)
  /// </summary>
  public static readonly Vector3 vm1m10 = new Vector3(-1, -1, 0);

  public static readonly Vector3 v10m1 = new Vector3(1, 0, -1);
  public static readonly Vector3 v00m1 = Vector3.back;
  public static readonly Vector3 v010 = Vector3.up;
  public static readonly Vector3 v0m10 = Vector3.down;
  public static readonly Vector3 vm100 = Vector3.left;
  public static readonly Vector3 v100 = Vector3.right;

  /// <summary>Return true if Vector2.zero, used to treat unset Vector as infinite Vector (e.g. in Boundary)</summary>
  public static bool ContainsIgnoreZero(this Vector2 vector, float value) =>
    vector == Vector2.zero || vector.Contains(value);

  /// <summary>
  ///   Check contain inclusively.
  /// </summary>
  public static bool Contains(this Vector2 vector, float value) => vector.x <= value && value <= vector.y;

  // public static bool Equals(this Vector3 vector1, Vector3 vector2, AxisFlag axis){
  // }

  /// <summary> Uses a Vector2 where x is min and y is max. </summary>
  public static float Clamp(this Vector2 vector, float value) => Mathf.Clamp(value, vector.x, vector.y);

  public static float Average(this Vector2 vector) => (vector.x + vector.y) / 2;

  public static float Length(this Vector2 vector) => Mathf.Abs(vector.x - vector.y);

  public static float Random(this Vector2 vector) => UnityEngine.Random.Range(vector.x, vector.y);

  public static int Random(this Vector2Int vector) => UnityEngine.Random.Range(vector.x, vector.y);

  public static Vector2 Offset(this Vector2 vector, float offset) => new Vector2(vector.x + offset, vector.y + offset);

  public static Vector3 WithX(this Vector3 vector, float newX) => new Vector3(newX, vector.y, vector.z);

  public static Vector3 WithXRandom(this Vector3 vector, float minX, float maxX) =>
    vector.WithX(UnityEngine.Random.Range(minX, maxX));

  public static Vector3 WithY(this Vector3 vector, float newY) => new Vector3(vector.x, newY, vector.z);
  public static Vector3 WithYZ(this Vector3 vector, float newValue) => new Vector3(vector.x, newValue, newValue);

  public static Vector3 WithYRandom(this Vector3 vector, float minY, float maxY) =>
    new Vector3(vector.x, UnityEngine.Random.Range(minY, maxY), vector.z);

  public static Vector3 WithNegativeY(this Vector3 vector) => new Vector3(vector.x, -Mathf.Abs(vector.y), vector.z);

  public static Vector3 WithPositiveY(this Vector3 vector) => new Vector3(vector.x, Mathf.Abs(vector.y), vector.z);

  public static Vector3 OffsetX(this Vector3 vector, float offset) =>
    new Vector3(vector.x + offset, vector.y, vector.z);

  public static Vector3 OffsetY(this Vector3 vector, float offset) =>
    new Vector3(vector.x, vector.y + offset, vector.z);

  public static Vector3 OffsetZ(this Vector3 vector, float offset) =>
    new Vector3(vector.x, vector.y, vector.z + offset);

  public static Vector3 WithZ(this Vector3 vector, float newZ) => new Vector3(vector.x, vector.y, newZ);

  public static Vector3 SetX(this Vector3 vector, float newX) => new Vector3(newX, vector.y, vector.z);

  public static Vector3 SetY(this Vector3 vector, float newY) => new Vector3(vector.x, newY, vector.z);

  public static Vector3 SetZ(this Vector3 vector, float newZ) => new Vector3(vector.x, vector.y, newZ);

  public static Vector2 GetXZ(this Vector3 vector) => new Vector2(vector.x, vector.z);

  /// <summary>
  ///   Return Vector3.zero if no flag.
  /// </summary>
  public static Vector3 ToVector3(this AxisFlag axisFlag) {
    if (axisFlag.HasFlags(AxisFlag.X, AxisFlag.Y, AxisFlag.Z)) return Vector3.one;
    if (!axisFlag.HasFlag(AxisFlag.X) && !axisFlag.HasFlag(AxisFlag.Y) && !axisFlag.HasFlag(AxisFlag.Z))
      return Vector3.zero;
    if (!axisFlag.HasFlag(AxisFlag.X) && axisFlag.HasFlag(AxisFlag.Y) && axisFlag.HasFlag(AxisFlag.Z)) return v011;
    if (!axisFlag.HasFlag(AxisFlag.X) && !axisFlag.HasFlag(AxisFlag.Y) && axisFlag.HasFlag(AxisFlag.Z)) return v001;
    if (!axisFlag.HasFlag(AxisFlag.X) && axisFlag.HasFlag(AxisFlag.Y) && !axisFlag.HasFlag(AxisFlag.Z)) return v010;
    if (axisFlag.HasFlag(AxisFlag.X) && !axisFlag.HasFlag(AxisFlag.Y) && axisFlag.HasFlag(AxisFlag.Z)) return v010;
    if (axisFlag.HasFlag(AxisFlag.X) && !axisFlag.HasFlag(AxisFlag.Y) && !axisFlag.HasFlag(AxisFlag.Z)) return v011;
    if (axisFlag.HasFlag(AxisFlag.X) && axisFlag.HasFlag(AxisFlag.Y) && !axisFlag.HasFlag(AxisFlag.Z)) return v001;
    return Vector3.zero;
  }

  /// <summary>
  ///   Scale all vectors in the array by the given factor.
  /// </summary>
  public static Vector3[] Scale(this Vector3[] vectors, Vector3 factor) {
    for (var i = 0; i < vectors.Length; i++) vectors[i].Scale(factor);

    return vectors;
  }

  /// <summary>
  ///   Add the offset to all vectors in the array.
  /// </summary>
  public static Vector3[] Offset(this Vector3[] vectors, Vector3 offset) {
    for (var i = 0; i < vectors.Length; i++) vectors[i] += offset;

    return vectors;
  }

  /// <summary>
  /// Origin is at (0, 0, 0) be default.
  /// </summary>
  public static bool IsInsideCircle(this Vector3 vector3, float radius, Vector3? origin = null) {
    origin ??= Vector3.zero;

    return (vector3 - origin.Value).sqrMagnitude <= radius * radius;
  }
}