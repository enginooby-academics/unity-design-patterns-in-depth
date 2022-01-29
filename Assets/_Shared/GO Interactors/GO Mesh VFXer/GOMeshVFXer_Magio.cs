#if MAGIO
using Magio;
using UnityEngine;

public class GOMeshVFXer_Magio : GOI_ComponentIsEffect_CacheEffect<GOMeshVFXer_Magio, MagioObjectEffect> {
  public override void Interact(GameObject go, MagioObjectEffect effect) {
    base.Interact(go, effect);
    // ? Why has to toggle the component at the first time to take effect
    var effectComponent = _interactedGos[go].Component;
    effectComponent.enabled = false;
    effectComponent.enabled = true;
  }
}
#endif
