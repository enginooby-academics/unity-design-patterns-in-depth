using UnityEngine;

public class TestIInteractable : MonoBehaviour, IInteractable<GOMeshVFXer> {
  public PSMeshRendererUpdater Effect;

  public GameObject GameObject => gameObject;

  public void OnInteracted() {
    GOMeshVFXer.Instance.Interact(gameObject, Effect);
  }
}
