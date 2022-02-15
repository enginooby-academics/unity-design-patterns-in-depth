using System;
using UnityEngine;
using static VectorUtils;

public static class TransformUtils {
  // ?
  public static bool Contains(this Transform transform, Bounds bounds, Bounds target) {
    return bounds.Contains(target.ClosestPoint(transform.position));
  }

  public static float DistanceFrom(this Transform transform, Vector3 targetPos) {
    // optimizer than Vector3.Distance()
    var x = Vector3.SqrMagnitude(transform.position - targetPos);
    return Mathf.Pow(x, .5f);
  }

  public static float DistanceFrom(this Transform transform, Transform targetTransform) =>
    transform.DistanceFrom(targetTransform.position);

  /// <summary>
  ///   Check if distance to target is less than given range.
  /// </summary>
  public static bool IsInRange(this Transform transform, Transform targetTransform, float range) {
    // optimizer than Vector3.Distance()
    var distanceSquare = Vector3.SqrMagnitude(transform.position - targetTransform.position);
    return distanceSquare <= range * range;
  }

  /// <summary>
  /// (DeltaTime multiplied)
  /// </summary>
  public static void RotateForward(this Transform transform, float speed) {
    transform.Rotate(transform.forward, speed * Time.deltaTime);
  }

  /// <summary>
  ///   Rotate local X (red axis) to target.
  /// </summary>
  public static void LookAtX(this Transform transform, Transform target) {
    transform.LookAtX(target.position);
  }

  /// <summary>
  ///   Rotate local Y (green axis) to target.
  /// </summary>
  public static void LookAtY(this Transform transform, Transform target) {
    transform.LookAtY(target.position);
  }

  /// <summary>
  ///   Rotate local Z (blue axis) to target.
  /// </summary>
  public static void LookAtZ(this Transform transform, Transform target) {
    transform.LookAtZ(target.position);
  }

  /// <summary>
  ///   Rotate local X (red axis) to destination.
  /// </summary>
  public static void LookAtX(this Transform transform, Vector3 dest) {
    transform.right = dest - transform.position;
  }

  /// <summary>
  ///   Rotate local Y (green axis) to destination.
  /// </summary>
  public static void LookAtY(this Transform transform, Vector3 dest) {
    transform.up = dest - transform.position;
  }

  /// <summary>
  ///   Rotate local Z (blue axis) to destination.
  /// </summary>
  public static void LookAtZ(this Transform transform, Vector3 dest) {
    transform.forward = dest - transform.position;
  }

  /// <summary>
  ///   [In-update] <br />
  ///   Translate on local X (included deltaTime).
  /// </summary>
  public static void MoveX(this Transform transform, float distance = 1f) {
    transform.Translate(v100 * Time.deltaTime * distance);
  }

  /// <summary>
  ///   Translate on local Y (included deltaTime).
  /// </summary>
  public static void MoveY(this Transform transform, float distance = 1f) {
    transform.Translate(v010 * distance * Time.deltaTime);
  }

  /// <summary>
  ///   This looping movement is based on Time.time so don't need extra direction flag in the MonoBehaviour<br />
  ///   However, if the speed is changed in runtime, the position may be teleported.
  /// </summary>
  public static void MoveYInRange(this Transform transform, float minY, float maxY, float speed = 1f) {
    var position = transform.position;
    var pos1 = position.WithY(minY);
    var pos2 = position.WithY(maxY);
    position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    transform.position = position;
    // transform.position = Vector3.Lerp(pos1, pos2, Mathf.PingPong(Time.time * speed, 1.0f));
  }

  /// <summary>
  ///   Translate on local Z (included deltaTime).
  /// </summary>
  public static void MoveZ(this Transform transform, float distance = 1f) {
    transform.Translate(v001 * Time.deltaTime * distance);
  }

  /// <summary>
  ///   Rotate local Y (green axis) and move to (stop at) destination (included deltaTime).
  /// </summary>
  public static void LookAtAndMoveY(this Transform transform, Vector3 dest, float distance = 1f) {
    transform.LookAtY(dest);
    transform.MoveY(distance);
  }

  /// <summary>
  ///   Rotate local Y (green axis) and move to (stop at) target (included deltaTime).
  /// </summary>
  public static void LookAtAndMoveY(this Transform transform, Transform target, float distance = 1f) {
    transform.LookAtY(target.position);
    transform.MoveY(distance);
  }

  /// <summary>
  ///   Return center position of the collider. If collider not exist, return transform position.
  /// </summary>
  public static Vector3 GetColliderCenter(this Transform transform) {
    if (transform.TryGetComponent(out Collider collider)) return collider.bounds.center;
    return transform.position;
  }

  /// <summary>
  ///   Destroy all child GameObjects safely.
  /// </summary>
  public static void DestroyChildren(this Transform transform) {
    transform.gameObject.GetComponentsInChildrenOnly<Transform>().DestroyGameObjects();
  }

  #region RESET =======================================================================================================================================================================

  /// <summary>
  ///   Reset position, rotation, scale
  /// </summary>
  public static void Reset(this Transform transform) {
    transform.ResetPosition();
    transform.ResetRotation();
    transform.ResetScale();
  }

  /// <summary>
  ///   Reset local position, local rotation, local scale
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

  #endregion RESET ====================================================================================================================================================================

  #region POSITION ===================================================================================================================================

  public static void SetPosX(this Transform transform, float x) {
    transform.position = transform.position.WithX(x);
  }

  public static void SetPosY(this Transform transform, float y) {
    transform.position = transform.position.WithY(y);
  }

  public static void SetPosZ(this Transform transform, float z) {
    transform.position = transform.position.WithZ(z);
  }

  public static void SetPos(this Transform transform, Vector3 pos) {
    transform.position = pos;
  }

  [Obsolete("Use " + nameof(SetPosX))]
  public static void PosX(this Transform transform, float x) => transform.SetPosX(x);

  [Obsolete("Use " + nameof(SetPosY))]
  public static void PosY(this Transform transform, float y) => transform.SetPosY(y);

  [Obsolete("Use " + nameof(SetPosZ))]
  public static void PosZ(this Transform transform, float z) => transform.SetPosZ(z);

  /// <summary>
  ///   E.g. Update (1, 1, 1) with (2, 2, 2) with Axis.XZ => (2, 1, 2)
  /// </summary>
  public static void UpdatePosOnAxis(this Transform transform, Transform target, AxisFlag axis) {
    if (axis.HasFlag(AxisFlag.X)) transform.SetPosX(target.position.x);
    if (axis.HasFlag(AxisFlag.Y)) transform.SetPosY(target.position.y);
    if (axis.HasFlag(AxisFlag.Z)) transform.SetPosZ(target.position.z);
  }

  /// <summary>
  ///   Copy position, rotation & localScale values from target Transform.
  /// </summary>
  public static void Copy(this Transform transform, Transform target) {
    transform.position = target.position;
    transform.rotation = target.rotation;
    transform.localScale = target.localScale;
  }


  /// <summary>
  ///   Copy localPosition, localRotation & localScale values from target Transform.
  /// </summary>
  public static void CopyLocal(this Transform transform, Transform target) {
    transform.localPosition = target.localPosition;
    transform.localRotation = target.localRotation;
    transform.localScale = target.localScale;
  }


  /// <summary>
  ///   Copy position, rotation & localScale values from target GameObject's Transform.
  /// </summary>
  public static void Copy(this Transform transform, GameObject target) {
    transform.position = target.transform.position;
    transform.rotation = target.transform.rotation;
    transform.localScale = target.transform.localScale;
  }

  /// <summary>
  /// Used for smooth position lerping.
  /// </summary>
  public static void SmoothApproach(this Transform transform, Vector3 pastPosition, Vector3 pastTargetPosition,
    Vector3 targetPosition,
    float delta) {
    if (Time.timeScale == 0 || float.IsNaN(delta) || float.IsInfinity(delta) || delta == 0 ||
        pastPosition == Vector3.zero || pastTargetPosition == Vector3.zero || targetPosition == Vector3.zero)
      return;

    var t = (Time.deltaTime * delta) + .00001f;
    var v = (targetPosition - pastTargetPosition) / t;
    var f = pastPosition - pastTargetPosition + v;
    var l = targetPosition - v + f * Mathf.Exp(-t);

    if (l != Vector3.negativeInfinity && l != Vector3.positiveInfinity && l != Vector3.zero)
      transform.position = l;
  }

  #endregion POSITION ================================================================================================================================

  #region SCALE ===================================================================================================================================

  /// <summary>
  ///   Multiply transform.localScale.y by the given factor.
  /// </summary>
  public static void MultiplyScaleY(this Transform transform, float factor) {
    transform.SetScaleY(transform.localScale.y * factor);
  }

  public static void SetScaleY(this Transform transform, float y) {
    transform.localScale = transform.localScale.WithY(y);
  }

  /// <summary>
  ///   Set localScale.x/y/z to the given value.
  /// </summary>
  public static void SetScale(this Transform transform, float value) {
    transform.localScale = Vector3.one * value;
  }

  /// <summary>
  ///   Set localScale.x/y/z to the given value.
  /// </summary>
  public static void SetScale(this MonoBehaviour monoBehaviour, float value) {
    monoBehaviour.transform.localScale = Vector3.one * value;
  }

  /// <summary>
  ///   Set localScale.x/y/z to the given value.
  /// </summary>
  public static void SetScale(this GameObject go, float value) {
    go.transform.localScale = Vector3.one * value;
  }

  #endregion SCALE ================================================================================================================================
}