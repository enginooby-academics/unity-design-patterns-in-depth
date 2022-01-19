// * Effect is setup by the component itself.
// * Prefab workflow. Interactor singleton need to be setup in scene.

using UnityEngine;

public abstract partial class GOI_ComponentIsEffect<TSelf, TComponent> {
  // Fallback in case the singleton component is not available or have not set up effects
  private void InitializeEffect() {
    if (_currentEffect) return;

    if (_effectPrefabs.IsSet()) {
      _currentEffect = _effectPrefabs[0];
    } else {
      // TODO: Find asset by type in project, if not found throw error
    }
  }

  public override void AwakeSingleton() {
    base.AwakeSingleton();
    InitializeEffect();
  }

  protected virtual TComponent AddOrGetCachedComponent(GameObject go) {
    TComponent component;

    if (_interactedGos.ContainsKey(go)) {
      component = _interactedGos[go].Component;
    } else {
      if (!go.TryGetComponent<TComponent>(out component)) {
        component = go.AddComponent<TComponent>();
        OnComponentAdded(go, component);
      }

      _interactedGos.Add(go, new GOIStruct<TComponent, TComponent>(component, _currentEffect));
    }

    component.enabled = true;
    return component;
  }
}