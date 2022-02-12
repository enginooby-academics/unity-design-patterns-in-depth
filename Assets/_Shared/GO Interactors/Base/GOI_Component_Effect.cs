using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

// TODO: Implement stackable effect
public abstract partial class GOI<TSelf, TComponent, TEffect> {
  [ValueDropdown(nameof(_effectPrefabs))] [SerializeField]
  protected TEffect _currentEffect;

  [SerializeField] [InlineEditor] protected List<TEffect> _effectPrefabs;

  protected new Dictionary<GameObject, GOIStruct<TComponent, TEffect>> _interactedGos =
    new Dictionary<GameObject, GOIStruct<TComponent, TEffect>>();

  public TEffect CurrentEffect => _currentEffect;
  public override List<GameObject> InteractedGos => _interactedGos.Keys.ToList();

  public override void IncrementEffect() {
    _currentEffect = _effectPrefabs.GetNext(_currentEffect);
  }

  public override void DecrementEffect() {
    _currentEffect = _effectPrefabs.GetPrevious(_currentEffect);
  }

  public abstract void Interact(GameObject go, TEffect effect);

  public override void Interact(GameObject go) {
    Interact(go, _currentEffect);
  }

  protected override void ClearInteractedGos() {
    _interactedGos.Clear();
  }

  // REFACTOR: Duplicated with parent class
  public override void InteractRestore(GameObject go) {
    if (_interactedGos.TryGetValue(go, out var cache)) SetComponentActive(cache.Component, true);
  }

  public override void InteractRevert(GameObject go) {
    if (_interactedGos.TryGetValue(go, out var cache)) SetComponentActive(cache.Component, false);
  }

  public override void InteractToggle(GameObject go) {
    if (_interactedGos.TryGetValue(go, out var cache))
      SetComponentActive(cache.Component, !GetComponentActive(cache.Component));
  }
}


public class GOIStruct<TComponent, TEffect> {
  public TComponent Component;
  public TEffect Effect;

  public GOIStruct(TComponent component, TEffect effect) {
    Component = component;
    Effect = effect;
  }
}