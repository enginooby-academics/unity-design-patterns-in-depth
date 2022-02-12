using UnityEngine;

public class SpawnPoint : MonoBehaviour {
  public float gizmosRadius = .2f;

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.yellow;
    Gizmos.DrawSphere(transform.position, gizmosRadius);
  }
}