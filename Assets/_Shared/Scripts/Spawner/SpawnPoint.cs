using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {
  public float gizmosRadius = .2f;

  void OnDrawGizmosSelected() {
    Gizmos.color = Color.yellow;
    Gizmos.DrawSphere(transform.position, gizmosRadius);
  }
}
