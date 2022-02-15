using UnityEngine;

public static class RigidBodyUtils {
  public static void SetVelocityX(this Rigidbody rb, float value) => rb.velocity = rb.velocity.WithX(value);

  public static void ZeroizeVelocityX(this Rigidbody rb) => rb.SetVelocityX(0f);

  public static void SetAngularVelocityYZ(this Rigidbody rb, float value) =>
    rb.angularVelocity = rb.angularVelocity.WithYZ(value);

  public static void ZeroizeAngularVelocityYZ(this Rigidbody rb) => rb.SetAngularVelocityYZ(0f);
}