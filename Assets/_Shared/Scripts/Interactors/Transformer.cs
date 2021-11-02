using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformer : MonoBehaviour {
  [SerializeField] MonoBehaviour currentPlayer;
  private Vector3 previousPlayerPos;
  private Selector selector;

  void Start() {
    selector = FindObjectOfType<Selector>();
  }

  void Update() {

  }

  // TODO: Process dest so that player do not collide w/ target
  public void MovePlayerTowards(GameObject target) {
    if (!currentPlayer) return;

    Vector3 rawDest = target.transform.position;
    Vector3 dest = new Vector3(rawDest.x, rawDest.y + 50f, rawDest.z);
    previousPlayerPos = currentPlayer.transform.position;
    currentPlayer.MoveTowards(dest, 1500f);
    // currentPlayer.MoveTowardsByRigidBody(dest, offset: 0f, speed: 100f);
  }

  public void RevertPlayerPosition() {
    currentPlayer.MoveTowards(previousPlayerPos, 1500f);
  }

  public void MoveTowardsPlayer(GameObject target) {
    if (!currentPlayer) return;

    target.transform.position = currentPlayer.transform.position;
    if (selector) FixMovedObjectForSelector(target);
  }

  // for selector to ray cast correctly after the target changing it position,
  // the target must setup rigidbody to force updating physics engine
  public void FixMovedObjectForSelector(GameObject target) {
    // Rigidbody rb = target.GetComponent<Rigidbody>() ?? target.AddComponent<Rigidbody>();
    Rigidbody rb = target.GetComponent<Rigidbody>();
    if (!rb) {
      rb = target.AddComponent<Rigidbody>();
      rb.useGravity = false; // optional
    }

    rb.interpolation = RigidbodyInterpolation.Interpolate; // required
  }
}
