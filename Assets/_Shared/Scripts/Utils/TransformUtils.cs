using UnityEngine;
using static VectorUtils;

// namespace ExtentionMethods {
public static class TransformUtils {
  /// <summary>
  /// Reset position, rotation, scale
  /// </summary>
  public static void Reset(this Transform transform) {
    transform.ResetPosition();
    transform.ResetRotation();
    transform.ResetScale();
  }

  /// <summary>
  /// Reset local position, local rotation, local scale
  /// </summary>
  public static void ResetLocal(this Transform transform) {
    transform.localPosition = Vector3.zero;
    transform.localScale = Vector3.one;
    transform.localEulerAngles = Vector3.zero;
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

  public static float DistanceFrom(this Transform transform, Vector3 targetPos) {
    // optimizer than Vector3.Distance()
    float x = Vector3.SqrMagnitude(transform.position - targetPos);
    return Mathf.Pow(x, .5f);
  }

  public static float DistanceFrom(this Transform transform, Transform targetTransform) {
    return transform.DistanceFrom(targetTransform.position);
  }

  /// <summary>
  /// Check if distance to target is less than given range.
  /// </summary>
  public static bool IsInRange(this Transform transform, Transform targetTransform, float range) {
    // optimizer than Vector3.Distance()
    float distanceSquare = Vector3.SqrMagnitude(transform.position - targetTransform.position);
    return distanceSquare <= range * range;
  }

  /// <summary>
  /// Rotate local X (red axis) to target.
  /// </summary>
  public static void LookAtX(this Transform transform, Transform target) {
    transform.LookAtX(target.position);
  }

  /// <summary>
  /// Rotate local Y (green axis) to target.
  /// </summary>
  public static void LookAtY(this Transform transform, Transform target) {
    transform.LookAtY(target.position);
  }

  /// <summary>
  /// Rotate local Z (blue axis) to target.
  /// </summary>
  public static void LookAtZ(this Transform transform, Transform target) {
    transform.LookAtZ(target.position);
  }

  /// <summary>
  /// Rotate local X (red axis) to destination.
  /// </summary>
  public static void LookAtX(this Transform transform, Vector3 dest) {
    transform.right = dest - transform.position;
  }

  /// <summary>
  /// Rotate local Y (green axis) to destination.
  /// </summary>
  public static void LookAtY(this Transform transform, Vector3 dest) {
    transform.up = dest - transform.position;
  }

  /// <summary>
  /// Rotate local Z (blue axis) to destination.
  /// </summary>
  public static void LookAtZ(this Transform transform, Vector3 dest) {
    transform.forward = dest - transform.position;
  }

  /// <summary>
  /// Translate on local X (included deltaTime).
  /// </summary>
  public static void MoveX(this Transform transform, float distance = 1f) {
    transform.Translate(v100 * Time.deltaTime * distance);
  }

  /// <summary>
  /// Translate on local Y (included deltaTime).
  /// </summary>
  public static void MoveY(this Transform transform, float distance = 1f) {
    transform.Translate(v010 * Time.deltaTime * distance);
  }

  /// <summary>
  /// Translate on local Z (included deltaTime).
  /// </summary>
  public static void MoveZ(this Transform transform, float distance = 1f) {
    transform.Translate(v001 * Time.deltaTime * distance);
  }

  /// <summary>
  /// Rotate local Y (green axis) and move to (stop at) destination (included deltaTime).
  /// </summary>
  public static void LookAtAndMoveY(this Transform transform, Vector3 dest, float distance = 1f) {
    transform.LookAtY(dest);
    transform.MoveY(distance);
  }

  /// <summary>
  /// Rotate local Y (green axis) and move to (stop at) target (included deltaTime).
  /// </summary>
  public static void LookAtAndMoveY(this Transform transform, Transform target, float distance = 1f) {
    transform.LookAtY(target.position);
    transform.MoveY(distance);
  }

  /// <summary>
  /// Return center position of the collider. If collider not exist, return transform position.
  /// </summary>
  public static Vector3 GetColliderCenter(this Transform transform) {
    if (transform.TryGetComponent<Collider>(out Collider collider)) {
      return collider.bounds.center;
    }
    return transform.position;
  }
}