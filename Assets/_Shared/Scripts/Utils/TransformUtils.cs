using UnityEngine;

// namespace ExtentionMethods {
public static class TransformUtils {

  /// <summary>Reset position, rotation, scale</summary>
  public static void Reset(this Transform transform) {
    transform.ResetPosition();
    transform.ResetRotation();
    transform.ResetScale();
  }

  public static void ResetPosition(this Transform transform) {
    transform.position = Vector3.zero;
  }

  public static void ResetRotation(this Transform transform) {
    transform.localRotation = Quaternion.identity;
  }

  public static void ResetScale(this Transform transform) {
    transform.localScale = new Vector3(1, 1, 1);
  }

  public static void PosX(this Transform transform, float x) {
    transform.position = new Vector3(x, transform.position.y, transform.position.z);
  }

  public static void PosY(this Transform transform, float y) {
    transform.position = new Vector3(transform.position.x, y, transform.position.z);
  }

  public static void PosZ(this Transform transform, float z) {
    transform.position = new Vector3(transform.position.x, transform.position.y, z);
  }

  /// <summary>E.g. Update (1, 1, 1) with (2, 2, 2) with Axis.XZ => (2, 1, 2)</summary>
  public static void UpdatePosOnAxis(this Transform transform, Transform target, AxisFlag axis) {
    if (axis.HasFlag(AxisFlag.X)) transform.PosX(target.position.x);
    if (axis.HasFlag(AxisFlag.Y)) transform.PosY(target.position.y);
    if (axis.HasFlag(AxisFlag.Z)) transform.PosZ(target.position.z);
  }

  /// <summary>
  /// Copy position, rotation & localScale values from target Transform.
  /// </summary>
  public static void CopyFrom(this Transform transform, Transform target) {
    transform.position = target.position;
    transform.rotation = target.rotation;
    transform.localScale = target.localScale;
  }

  /// <summary>
  /// Copy position, rotation & localScale values from target GameObject's Transform.
  /// </summary>
  public static void CopyFrom(this Transform transform, GameObject target) {
    transform.position = target.transform.position;
    transform.rotation = target.transform.rotation;
    transform.localScale = target.transform.localScale;
  }
}