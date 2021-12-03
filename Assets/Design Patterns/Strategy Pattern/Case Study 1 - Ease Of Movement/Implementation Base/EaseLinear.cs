using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Strategy.Base {
  public class EaseLinear : IMovementEase {
    public void Move(GameObject gameObject, Vector3 dest, float speed) {
      Debug.Log("Moving with linear ease");
      gameObject.transform.DOMove(dest, speed).SetSpeedBased(true).SetEase(Ease.Linear);
    }
  }
}
