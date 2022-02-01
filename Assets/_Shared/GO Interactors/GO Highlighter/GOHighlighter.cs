#if URP_OUTLINE
using EPOOutline;
using UnityEngine;

public class GOHighlighter : GOI_ComponentIsEffect_CacheEffect<GOHighlighter, Outlinable> {
  public override void Interact(GameObject go, Outlinable effect) {
    if (_interactedGos.TryGetValue(go, out var cache)) {
      cache.Component.enabled = true;
      if (effect.Equals(cache.Effect)) return;

      Destroy(cache.Component);
      _interactedGos.Remove(go);
    }

    AddOutlinable(go, effect);
  }

  // instantiate a new Outlinable to use as template instead of directly using SerializeField prefab 
  // to prevent change linking (update in this GameObject will cause update in effect prefab)
  // REFACTOR: Generalize AddComponentEffectByCloning
  private void AddOutlinable(GameObject go, Outlinable effect) {
    var outlinableTemplate = Instantiate(effect);
    var outlinable = go.AddComponent<Outlinable>().GetLinkedCopyOf(outlinableTemplate);
    var outlineTarget = new OutlineTarget(go.GetComponent<Renderer>());
    var cache = new GOIStruct<Outlinable, Outlinable>(outlinable, effect);

    Destroy(outlinableTemplate.gameObject);
    outlinable.OutlineTargets.Add(outlineTarget);
    if (!_interactedGos.ContainsKey(go)) {
      _interactedGos.Add(go, cache);
    }
  }
}
#endif