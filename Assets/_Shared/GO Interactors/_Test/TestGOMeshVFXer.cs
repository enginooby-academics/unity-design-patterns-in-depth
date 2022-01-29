#if ASSET_MESH_EFFECTS
using UnityEngine;

public class TestGOMeshVFXer : MonoBehaviour {
  public bool Enable;
  public PSMeshRendererUpdater Effect;

  private void OnMouseDown() {
    if (!Enable) return;
    MonoBehaviourSingleton<GOMeshVFXer_MeshEffects>.Instance.Interact(gameObject, Effect);
  }
}
#endif
