using UnityEngine;

namespace Strategy.Base {
  public interface IMovementEase {
    void Move(GameObject gameObject, Vector3 dest, float speed);
  }
}