using DG.Tweening;
using UnityEngine;

public static class DOTweenUtils {
  public static void ShakePosition(this Transform transform) {
    // TODO: parameterize
    transform.DOShakePosition(1f).SetLoops(-1);
  }

  public static void ShakeRotation(this Transform transform) {
    transform.DOShakeRotation(1f).SetLoops(-1);
  }

  public static void ShakeScale(this Transform transform) {
    transform.DOShakeScale(1f).SetLoops(-1);
  }
}