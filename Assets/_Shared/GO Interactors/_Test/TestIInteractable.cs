using UnityEngine;

public class TestIInteractable : MonoBehaviour, IInteractable<GOMeshVFXer_MeshEffects> {
  public PSMeshRendererUpdater Effect;

  public GameObject GameObject => gameObject;

  public void OnInteracted() {
    GOMeshVFXer_MeshEffects.Instance.Interact(gameObject, Effect);
  }
}
