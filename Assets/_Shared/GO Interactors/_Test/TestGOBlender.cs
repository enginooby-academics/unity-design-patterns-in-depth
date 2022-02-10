#if ASSET_BLEND_MODES
using BlendModes;
using UnityEngine;

public class TestGOBlender : MonoBehaviour {
  public bool Enable;
  public BlendMode BlendMode;

  private void OnMouseDown() {
    if (!Enable) return;
    // MonoBehaviourSingleton<GOBlender>.Instance.Interact(gameObject, BlendMode);
    GOBlender.Instance.Interact(gameObject, BlendMode); // thanks to overided Instance in base
  }
}
#endif