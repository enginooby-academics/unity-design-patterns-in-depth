#if ASSET_DOTWEEN
using DG.Tweening;
#endif
using UnityEngine;

public static class DOTweenUtils {
  public static void ShakePosition(this Transform transform, float strength = 1f) {
#if ASSET_DOTWEEN
    // TODO: parameterize
    transform.DOShakePosition(strength).SetLoops(-1);
#endif
  }

  public static void ShakeRotation(this Transform transform) {
#if ASSET_DOTWEEN
    transform.DOShakeRotation(1f).SetLoops(-1);
#endif
  }

  public static void ShakeScale(this Transform transform) {
#if ASSET_DOTWEEN
    transform.DOShakeScale(1f).SetLoops(-1);
#endif
  }
}