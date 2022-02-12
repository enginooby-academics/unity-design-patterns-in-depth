#if ASSET_BLEND_MODES
using BlendModes;
using UnityEngine;

// REFACTOR: create wrapper for 3rd party asset if using muliple similar assets
// TODO
//+ Create customized effects

/// <summary>
/// * Use cases: colorize GOs, see through, un-highlight
/// </summary>
public partial class GOBlender : GOI_EffectIsEnum<GOBlender, BlendModeEffect, BlendMode, Material> {
  protected override BlendMode InitEffectEnum() => BlendMode.Divide;

  protected override void OnComponentAdded(GameObject go, BlendModeEffect component) {
    component.SetShaderFamily("LwrpUnlitTransparent"); // ! String
  }

  protected override Material CacheObject(GameObject go) {
    return go.GetComponent<Renderer>().material;
  }

  public override void Interact(GameObject go, BlendMode blendMode) {
    AddOrGetCachedComponent(go).SetBlendMode(blendMode);
  }

  public override void InteractRestore(GameObject go) {
    //? implement caching old effect (use different data structure for _interactedGos)
    // old blend mode is not cached to restore, so for now, just set all to current blend mode 
    Interact(go);
  }

  // TODO: Handle blended GO in edit time
  public override void InteractRevert(GameObject go) {
    if (_interactedGos.TryGetValue(go, out var cache)) {
      cache.Component.enabled = false;
      go.GetComponent<Renderer>().material = cache.Cache;
    }
  }

  public override void InteractToggle(GameObject go) {
  }
}
#endif