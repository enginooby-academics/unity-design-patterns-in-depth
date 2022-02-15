using UnityEngine;

public static class RandomUtils {
  public static float RandomTwoSides(this float range) => Random.Range(-Mathf.Abs(range), Mathf.Abs(range));

  public static bool Percent(this int percent) => Random.Range(0, 100) < percent;

  public static bool Percent(this float percent) => Random.Range(0f, 100f) < percent;

  /// <summary>E.g. Vector (1, 1, 1) with offset range (1, 2, 3) results (0->2, -1->3, -2->4) </summary>
  public static Vector3 RandomOffset(this Vector3 vector, Vector3? offsetRange = null) {
    var offset = offsetRange == null
      ? Vector3.zero
      : new Vector3(offsetRange.Value.x.RandomTwoSides(), offsetRange.Value.y.RandomTwoSides(),
        offsetRange.Value.z.RandomTwoSides());

    return vector + offset;
  }

  /// <summary>
  ///   E.g. Vector (1, 1, 1) return between vector (-1, -1, -1) and (1, 1, 1).
  /// </summary>
  public static Vector3 RandomRange(this Vector3 vector) {
    var randomX = vector.x.RandomTwoSides();
    var randomY = vector.y.RandomTwoSides();
    var randomZ = vector.z.RandomTwoSides();
    return new Vector3(randomX, randomY, randomZ);
  }
}