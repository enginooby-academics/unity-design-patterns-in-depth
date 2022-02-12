using UnityEngine;

public abstract partial class GOI_ComponentIsEffect_CacheEffect<TSelf, TComponent> {
  /// <summary>
  ///   Default implementation: if cached, set active. Otherwise, copy (unlinked) effect to GO.
  /// </summary>
  public override void Interact(GameObject go, TComponent effect) {
    if (_interactedGos.TryGetValue(go, out var cache)) {
      SetComponentActive(cache.Component, true);
      if (effect.Equals(cache.Effect)) return;
    }

    var componentEffect = effect.CopyTo(go);
    var newCache = new GOIStruct<TComponent, TComponent>(componentEffect, effect);

    // UTIL: implement TryAdd
    if (!_interactedGos.ContainsKey(go)) _interactedGos.Add(go, newCache);
  }
}