using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract partial class GOI<TSelf, TComponent> {
  protected new Dictionary<GameObject, TComponent> _interactedGos = new Dictionary<GameObject, TComponent>();
  public override List<GameObject> InteractedGos => _interactedGos.Keys.ToList();

  protected override void ClearInteractedGos() {
    _interactedGos.Clear();
  }

  protected virtual void SetComponentActive(TComponent component, bool isActive) {
    component.enabled = isActive;
  }

  protected virtual bool GetComponentActive(TComponent component) => component.enabled;

  protected virtual void OnComponentAdded(GameObject go, TComponent component) {
  }
}