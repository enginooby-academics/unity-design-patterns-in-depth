using System.Collections;
using UnityEngine;
using static VectorUtils;

public static class MovementUtils {
  // ? Use MonoBehaviour or transform
  /// <summary>Add world-space position X to distance</summary>
  public static void MoveXWorld(this MonoBehaviour monoBehaviour, float distance = 1f) {
    monoBehaviour.transform.MoveXWorld(distance);
  }

  /// <summary>Add world-space position Y to distance</summary>
  public static void MoveYWorld(this MonoBehaviour monoBehaviour, float distance = 1f) {
    monoBehaviour.transform.MoveYWorld(distance);
  }

  /// <summary>Add world-space position Z to distance</summary>
  public static void MoveZWorld(this MonoBehaviour monoBehaviour, float distance = 1f) {
    monoBehaviour.transform.MoveZWorld(distance);
  }

  /// <summary>Add world-space position X to distance</summary>
  public static void MoveXWorld(this Transform transform, float distance = 1f) {
    transform.Translate(v100 * Time.deltaTime * distance, Space.World);
  }

  /// <summary>Add world-space position Y to distance</summary>
  public static void MoveYWorld(this Transform transform, float distance = 1f) {
    transform.Translate(v010 * Time.deltaTime * distance, Space.World);
  }

  /// <summary>Add world-space position Z to distance</summary>
  public static void MoveZWorld(this Transform transform, float distance = 1f) {
    transform.Translate(v001 * Time.deltaTime * distance, Space.World);
  }

  public static void MoveWorld(this MonoBehaviour monoBehaviour, Vector3 distances) {
    monoBehaviour.transform.MoveWorld(distances);
  }

  public static void MoveWorld(this Transform transform, Vector3 distances) {
    transform.MoveXWorld(distances.x);
    transform.MoveYWorld(distances.y);
    transform.MoveZWorld(distances.z);
  }

  /// <summary>
  ///   Translate on local X (included deltaTime).
  /// </summary>
  public static void MoveX(this MonoBehaviour monoBehaviour, float distance = 1f) {
    monoBehaviour.transform.MoveX(distance);
  }

  /// <summary>
  ///   Translate on local Y (included deltaTime).
  /// </summary>
  public static void MoveY(this MonoBehaviour monoBehaviour, float distance = 1f) {
    monoBehaviour.transform.MoveY(distance);
  }

  /// <summary>
  ///   Translate on local Z (included deltaTime).
  /// </summary>
  public static void MoveZ(this MonoBehaviour monoBehaviour, float distance = 1f) {
    monoBehaviour.transform.MoveZ(distance);
  }

  public static void MoveXInBound(this MonoBehaviour monoBehaviour, float distance, Vector2 range) {
    monoBehaviour.transform.Translate(v100 * Time.deltaTime * distance);
    var posX = monoBehaviour.transform.position.x;
    if (range.Contains(posX)) return;
    monoBehaviour.transform.SetPosX(Mathf.Clamp(posX, range.x, range.y));
  }

  ///<summary>Rotates the object around the Y axis by the number of degrees defined by the given angle.</summary>
  public static void RotateY(this Transform transform, float angle) {
    transform.Rotate(v010, angle * Time.deltaTime);
  }

  ///<summary>Rotates the object around the X axis by the number of degrees defined by the given angle.</summary>
  public static void RotateX(this Transform transform, float angle) {
    transform.Rotate(v100, angle * Time.deltaTime);
  }

  ///<summary>Rotates the object around the Z axis by the number of degrees defined by the given angle.</summary>
  public static void RotateZ(this Transform transform, float angle) {
    transform.Rotate(v001, angle * Time.deltaTime);
  }

  ///<summary>Rotates the object around the 3 axes by the number of degrees defined by the given angle.</summary>
  public static void Rotate(this Transform transform, Vector3 angles) {
    transform.RotateX(angles.x);
    transform.RotateY(angles.y);
    transform.RotateZ(angles.z);
  }

  // TODO: Remove movement methods from MonoBehaviour (feature envy)
  ///<summary>Rotates the object around the Y axis by the number of degrees defined by the given angle.</summary>
  public static void RotateY(this MonoBehaviour monoBehaviour, float angle) {
    monoBehaviour.transform.Rotate(v010, angle * Time.deltaTime);
  }

  ///<summary>Rotates the object around the X axis by the number of degrees defined by the given angle.</summary>
  public static void RotateX(this MonoBehaviour monoBehaviour, float angle) {
    monoBehaviour.transform.Rotate(v100, angle * Time.deltaTime);
  }

  ///<summary>Rotates the object around the Z axis by the number of degrees defined by the given angle.</summary>
  public static void RotateZ(this MonoBehaviour monoBehaviour, float angle) {
    monoBehaviour.transform.Rotate(v001, angle * Time.deltaTime);
  }

  ///<summary>Rotates the object around the 3 axes by the number of degrees defined by the given angle.</summary>
  public static void Rotate(this MonoBehaviour monoBehaviour, Vector3 angles) {
    monoBehaviour.RotateX(angles.x);
    monoBehaviour.RotateY(angles.y);
    monoBehaviour.RotateZ(angles.z);
  }

  /// <summary>Rotate (slerp) to an angle destination. Call in Update()</summary>
  public static void RotateTowards(this MonoBehaviour monoBehaviour, Quaternion dest, float speed = .1f) {
    monoBehaviour.transform.rotation = Quaternion.RotateTowards(monoBehaviour.transform.rotation, dest, speed);
  }

  /// <summary>Reset rotation (slerp). Call in Update()</summary>
  public static void RotateTowardsIdentity(this MonoBehaviour monoBehaviour, float speed = .1f) {
    monoBehaviour.transform.rotation =
      Quaternion.RotateTowards(monoBehaviour.transform.rotation, Quaternion.identity, speed);
  }

  /// <summary>Move (slerp) to a destination. Using Coroutine so don't invoke in <c>Update()</c></summary>
  public static void MoveTowards(this MonoBehaviour monoBehaviour, Vector3 dest, float speed = 1f) {
    monoBehaviour.StartCoroutine(MoveTowardsEnumerator(monoBehaviour.transform, dest, speed));
  }

  // FIX: does not move
  public static void MoveTowardsByRigidBody(
    this MonoBehaviour monoBehaviour,
    Vector3 dest,
    float offset = 0f,
    float speed = 1f) {
    if (!(Vector3.Distance(monoBehaviour.transform.position, dest) > offset)) return;

    var direction = dest - monoBehaviour.transform.position;
    monoBehaviour.GetComponent<Rigidbody>().AddRelativeForce(direction.normalized * speed, ForceMode.Force);
    if (monoBehaviour.GetComponent<Rigidbody>()) "Move".Log();
  }

  private static IEnumerator MoveTowardsEnumerator(Transform transform, Vector3 dest, float speed) {
    while (transform.position != dest) {
      transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);

      // wait until next frame
      yield return null;
    }
  }
}