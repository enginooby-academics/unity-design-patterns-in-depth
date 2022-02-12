using UnityEngine;
#if ASSET_DOTWEEN
using DG.Tweening;
#endif

namespace Strategy.Base {
  public class EaseInSine : IMovementEase {
    public void Move(GameObject gameObject, Vector3 dest, float speed) {
      Debug.Log("Moving with in-sine ease");
#if ASSET_DOTWEEN
      gameObject.transform.DOMove(dest, speed).SetSpeedBased(true).SetEase(Ease.InSine);
#endif
    }
  }
}