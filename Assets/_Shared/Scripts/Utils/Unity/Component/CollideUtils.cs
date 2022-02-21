using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public static class CollideUtils {
  // ? Rename: OverlapBounds
  public static bool Overlap(this Collider collider, Collider target) =>
    collider.transform.Contains(collider.bounds, target.bounds);

  // ? API design: shorten common code (gameObject is cut off in bellow methods)
  public static T GetComponent<T>(this Collision collision) where T : Component =>
    collision.gameObject.GetComponent<T>();

  public static T GetComponent<T>(this Collider collider) where T : Component => collider.gameObject.GetComponent<T>();

  public static bool CompareTag(this Collider collider, string tag) => collider.gameObject.CompareTag(tag);

  public static bool CompareTag(this Collision collision, string tag) => collision.gameObject.CompareTag(tag);

  /// <summary>
  ///   Does GameObject of the given collider has one in the given tags?
  /// </summary>
  public static bool CompareTag(this Collider collider, params string[] tags) => tags.Any(collider.CompareTag);

  /// <summary>
  ///   Does GameObject of the given collision has one in the given tags?
  /// </summary>
  public static bool CompareTag(this Collision collision, params string[] tags) => tags.Any(collision.CompareTag);

  // ? API design: comparision/validation with actions on true/false 
  /// <param name="trueAction">Action invoked if tag is matched.</param>
  /// <param name="falseAction">Action invoked if tag isn't matched.</param>
  /// <returns>Is tag matched?</returns>
  public static bool CompareTag(
    this Collider collider,
    string tag,
    [CanBeNull] Action trueAction = null,
    [CanBeNull] Action falseAction = null) {
    var isTagMatched = collider.gameObject.CompareTag(tag);

    if (isTagMatched)
      trueAction?.Invoke();
    else
      falseAction?.Invoke();

    return isTagMatched;
  }

  /// <param name="trueAction">Action of this collider invoked if tag is matched.</param>
  /// <param name="falseAction">Action of this collider invoked if tag isn't matched.</param>
  /// <returns>Is tag matched?</returns>
  public static bool CompareTag(
    this Collider collider,
    string tag,
    [CanBeNull] Action<Collider> trueAction = null,
    [CanBeNull] Action<Collider> falseAction = null) {
    var isTagMatched = collider.gameObject.CompareTag(tag);

    if (isTagMatched)
      trueAction?.Invoke(collider);
    else
      falseAction?.Invoke(collider);

    return isTagMatched;
  }
}