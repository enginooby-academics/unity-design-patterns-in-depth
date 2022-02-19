using System;
using UnityEngine;
using Random = UnityEngine.Random;

// TODO: Move each method to its categorize and remove this class
public static class RandomUtils {
  /// <summary>
  ///   E.g., Given 3 return random number in range (-3, 3).
  /// </summary>
  public static float RandomTwoSides(this float range) => Random.Range(-Mathf.Abs(range), Mathf.Abs(range));

  /// <summary>
  ///   Out of 100%.
  /// </summary>
  public static bool Percent<T>(this T percent)
    where T : unmanaged,
    IComparable,
    IComparable<T>,
    IConvertible,
    IEquatable<T>,
    IFormattable =>
    percent.CompareTo(Random.Range(0f, 100f)) > 0;

  /// <summary>
  ///   E.g., Vector (1, 1, 1) with offset range (1, 2, 3) results (0->2, -1->3, -2->4)
  /// </summary>
  public static Vector3 RandomOffset(this Vector3 vector, Vector3? offsetRange = null) {
    var offset = offsetRange == null
      ? Vector3.zero
      : new Vector3(offsetRange.Value.x.RandomTwoSides(), offsetRange.Value.y.RandomTwoSides(),
        offsetRange.Value.z.RandomTwoSides());

    return vector + offset;
  }

  /// <summary>
  ///   E.g., Vector (1, 1, 1) return between vector (-1, -1, -1) and (1, 1, 1).
  /// </summary>
  public static Vector3 RandomRange(this Vector3 vector) {
    var randomX = vector.x.RandomTwoSides();
    var randomY = vector.y.RandomTwoSides();
    var randomZ = vector.z.RandomTwoSides();
    return new Vector3(randomX, randomY, randomZ);
  }
}