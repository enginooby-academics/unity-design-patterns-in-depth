using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// TODO:
// + rotational speed/acceleration w/ max speed
// + random range for params (speed/acceleration)
// + loop (w/ time)
// + local vs. World-space
// + OnCollision/Spawn VFX, SFX, event 

public class ArchievedProjectile : MonoBehaviour {
  #region TRANSLATION
  [SerializeField] Vector3 speed;
  [SerializeField] Vector3 translationalAcceleration;
  private bool isStopping = false;
  #endregion

  #region ROTATION
  #endregion

  #region EVENT
  #endregion

  void Update() {
    if (!isStopping) this.MoveWorld(distances: speed);
  }
  void OnDrawGizmosSelected() {
    Vector3 pos = transform.position;
    Vector3 dest = new Vector3(pos.x + speed.x, pos.y + speed.y, pos.z + speed.z);
    Gizmos.color = Color.magenta;
    Gizmos.DrawLine(pos, dest);
  }

  public void Stop() {
    isStopping = true;
  }
}
