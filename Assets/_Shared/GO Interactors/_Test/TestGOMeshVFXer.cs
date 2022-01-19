using UnityEngine;

public class TestGOMeshVFXer : MonoBehaviour {
  public bool Enable;
  public PSMeshRendererUpdater Effect;

  private void OnMouseDown() {
    if (!Enable) return;
    MonoBehaviourSingleton<GOMeshVFXer>.Instance.Interact(gameObject, Effect);
  }
}
