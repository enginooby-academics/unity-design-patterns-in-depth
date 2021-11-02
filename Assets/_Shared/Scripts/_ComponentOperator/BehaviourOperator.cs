using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourOperator<T> : ComponentOperator<Behaviour> where T : Behaviour { // Behaviour
  [SerializeField] protected T component;

  public virtual void DisableComponent() {
    component.enabled = false;
  }

  void Start() {
    Reset();
  }

  private void Reset() {
    // System.Type componentType = component.GetType();
    System.Type componentType = typeof(T);
    // component = component ?? GetComponent(componentType);
  }
}
