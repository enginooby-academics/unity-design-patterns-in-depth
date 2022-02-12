using UnityEngine;

public class ColliderOperator : ComponentOperator<Collider> {
  protected override void Reset() {
    if (_component) return;

    if (!gameObject.TryGetComponent(out _component)) _component = gameObject.AddComponent<MeshCollider>();
  }

  public void DisableCollider() {
    _component.enabled = false;
  }
}