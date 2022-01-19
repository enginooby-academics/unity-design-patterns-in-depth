using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract partial class GOI<TSelf, TComponent, TEffect, TCache> {
  protected new Dictionary<GameObject, GOIStruct<TComponent, TEffect, TCache>> _interactedGos = new Dictionary<GameObject, GOIStruct<TComponent, TEffect, TCache>>();
  protected override void ClearInteractedGos() => _interactedGos.Clear();
  public override List<GameObject> InteractedGos => _interactedGos.Keys.ToList();


  /// <summary>
  /// Cache necessary Object of the GO to implement reverting method.
  /// </summary>
  protected abstract TCache CacheObject(GameObject go);

  public TComponent AddOrGetCachedComponent(GameObject go) {
    TComponent component;

    if (_interactedGos.ContainsKey(go)) {
      component = _interactedGos[go].Component;
    } else {
      var cachedObject = CacheObject(go) as TCache;

      if (!go.TryGetComponent<TComponent>(out component)) {
        component = go.AddComponent<TComponent>();
        OnComponentAdded(go, component);
      }

      var cache = new GOIStruct<TComponent, TEffect, TCache>(component, _currentEffect, cachedObject);
      _interactedGos.Add(go, cache);
    }

    component.enabled = true;

    return component;
  }
}

public class GOIStruct<TComponent, TEffect, TCache> : GOIStruct<TComponent, TEffect> {
  public TCache Cache;

  public GOIStruct(TComponent component, TEffect effect, TCache cache) : base(component, effect) {
    Cache = cache;
  }
}