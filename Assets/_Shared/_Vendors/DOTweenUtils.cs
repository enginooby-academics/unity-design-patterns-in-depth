using UnityEngine;
#if ASSET_DOTWEEN
using DG.Tweening;
#endif

// TODO: Implement fallbacks if asset is not available

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

  public static void MoveX(this Transform transform, float endValue, float duration) {
#if ASSET_DOTWEEN
    transform.DOMoveX(endValue, duration);
#endif
  }

  public static void MoveY(this Transform transform, float endValue, float duration) {
#if ASSET_DOTWEEN
    transform.DOMoveY(endValue, duration);
#endif
  }
}