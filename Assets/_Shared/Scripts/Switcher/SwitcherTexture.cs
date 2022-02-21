using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

public class SwitcherTexture : Switcher<Texture> {
  [PropertyOrder(-1)] [SerializeField] private Renderer meshRenderer;

  public override void Init() {
    meshRenderer = GetComponent<Renderer>();
  }

  public override void Switch() {
    if (!meshRenderer) return;
    // CONSIDER: implement for different maps
    meshRenderer.material.SetTexture("_BaseMap", collection.CurrentItemOrFirst);
  }
}