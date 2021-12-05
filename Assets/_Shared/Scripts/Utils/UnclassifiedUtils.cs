using System;
using UnityEngine;

public static class UnclassifiedUtils {
  public static bool EqualIgnoreCase(this string string1, string string2) {
    return string.Equals(string1, string2, StringComparison.OrdinalIgnoreCase);
  }

  /// <summary>
  /// Return true if value just below min, false if value just above max, null if value in range.
  /// </summary>
  public static bool? ReachingMinOrMax(this float value, float min, float max) {
    if (value <= min) return true;
    if (value >= max) return false;
    return null;
  }

  /// <summary>
  /// Return true if y position just below min, false if y position just above max, null if y position in range.
  /// </summary>
  public static bool? ReachingYMinOrMax(this Transform transform, float min, float max) {
    if (transform.position.y <= min) return true;
    if (transform.position.y >= max) return false;
    return null;
  }
}

