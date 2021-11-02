using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderOperator : ComponentOperator<Collider> {
  public override void DisableComponent() {
    component.enabled = false;
  }
}
