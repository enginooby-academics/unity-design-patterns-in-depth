using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Strategy.Base {
  public class EaseInSine : IMovementEase {
    public void Move(GameObject gameObject, Vector3 dest, float speed) {
      Debug.Log("Moving with in-sine ease");
      gameObject.transform.DOMove(dest, speed).SetSpeedBased(true).SetEase(Ease.InSine);
    }
  }
}
