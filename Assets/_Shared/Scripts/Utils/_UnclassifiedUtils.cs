using System;
using System.Linq;
using UnityEngine;

public static class _UnclassifiedUtils {
  public static bool Overlap(this Collider collider, Collider target) =>
    collider.transform.Contains(collider.bounds, target.bounds);

  public static T GetComponent<T>(this Collision collision) where T : Component =>
    collision.gameObject.GetComponent<T>();

  public static T GetComponent<T>(this Collider collider) where T : Component => collider.gameObject.GetComponent<T>();

  public static bool CompareTag(this Collider collider, string tag) => collider.gameObject.CompareTag(tag);

  public static bool CompareTag(this Collider collider, string tag, Action trueAction, Action falseAction) {
    var isTagMatched = collider.gameObject.CompareTag(tag);
    if (isTagMatched)
      trueAction?.Invoke();
    else
      falseAction?.Invoke();

    return isTagMatched;
  }

  public static bool CompareTag(
    this Collider collider,
    string tag,
    Action<Collider> trueAction,
    Action<Collider> falseAction) {
    var isTagMatched = collider.gameObject.CompareTag(tag);
    if (isTagMatched)
      trueAction?.Invoke(collider);
    else
      falseAction?.Invoke(collider);

    return isTagMatched;
  }

  /// <summary>
  /// Check if GameObject of the given collider has one in the given tags
  /// </summary>
  public static bool CompareTag(this Collider collider, params string[] tags) => tags.Any(collider.CompareTag);

  public static bool CompareTag(this Collision collision, string tag) => collision.gameObject.CompareTag(tag);

  /// <summary>
  ///   Return 1 if true, 0 if false.
  /// </summary>
  public static int ToInt(this bool value, int trueValue = 1, int falseValue = 0) => value ? trueValue : falseValue;

  /// <summary>
  ///   Return 1 if true, -1 if false.
  /// </summary>
  public static int To1OrMinus1(this bool value) => value ? 1 : -1;

  /// <summary>
  ///   Return true if value just below min, false if value just above max, null if value in range.
  /// </summary>
  public static bool? ReachingMinOrMax(this float value, float min, float max) {
    if (value <= min) return true;
    if (value >= max) return false;
    return null;
  }

  /// <summary>
  ///   Return true if y position just below min, false if y position just above max, null if y position in range.
  /// </summary>
  public static bool? ReachingYMinOrMax(this Transform transform, float min, float max) {
    if (transform.position.y <= min) return true;
    if (transform.position.y >= max) return false;
    return null;
  }

  public static Rect GetHalfTop(this Rect rect) => new Rect(rect.x, rect.y, rect.width, rect.height / 2);

  public static Rect GetHalfBottom(this Rect rect) =>
    new Rect(rect.x, rect.y + rect.height / 2, rect.width, rect.height / 2);
}