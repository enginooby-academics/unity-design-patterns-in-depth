// * Effect is setup by the component itself.
// * Prefab workflow. Interactor singleton need to be setup in scene.

using Enginooby.Utils;
using UnityEngine;

public abstract partial class GOI_ComponentIsEffect<TSelf, TComponent> {
  // Fallback in case the singleton component is not available or have not set up effects
  private void InitializeEffect() {
    if (_currentEffect || _effectPrefabs.IsNullOrEmpty()) return;

    _currentEffect = _effectPrefabs[0];
  }

  public override void AwakeSingleton() {
    base.AwakeSingleton();
    InitializeEffect();
  }

  protected virtual TComponent AddOrGetCachedComponent(GameObject go) {
    TComponent component;

    if (_interactedGos.ContainsKey(go)) {
      component = _interactedGos[go].Component;
    }
    else {
      if (!go.TryGetComponent(out component)) {
        component = go.AddComponent<TComponent>();
        OnComponentAdded(go, component);
      }

      _interactedGos.Add(go, new GOIStruct<TComponent, TComponent>(component, _currentEffect));
    }

    component.enabled = true;
    return component;
  }
}