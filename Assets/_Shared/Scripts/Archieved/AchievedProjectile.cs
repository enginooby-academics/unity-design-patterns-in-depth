using UnityEngine;

// TODO:
// + rotational speed/acceleration w/ max speed
// + random range for params (speed/acceleration)
// + loop (w/ time)
// + local vs. World-space
// + OnCollision/Spawn VFX, SFX, event 

public class ArchievedProjectile : MonoBehaviour {
  private void Update() {
    if (!isStopping) this.MoveWorld(speed);
  }

  private void OnDrawGizmosSelected() {
    var pos = transform.position;
    var dest = new Vector3(pos.x + speed.x, pos.y + speed.y, pos.z + speed.z);
    Gizmos.color = Color.magenta;
    Gizmos.DrawLine(pos, dest);
  }

  public void Stop() {
    isStopping = true;
  }

  #region TRANSLATION

  [SerializeField] private Vector3 speed;
  [SerializeField] private Vector3 translationalAcceleration;
  private bool isStopping;

  #endregion

  #region ROTATION

  #endregion

  #region EVENT

  #endregion
}