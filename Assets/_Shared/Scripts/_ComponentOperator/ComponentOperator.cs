using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ComponentOperator<T> : MonoBehaviourBase where T : Component { // Behaviour
  [SerializeField] protected T component;

  public virtual void DisableComponent() {
    if (typeof(T) == typeof(Behaviour))
      // ! canot cast, return null
      (component as Behaviour).enabled = false;
  }

  void Start() {
    Reset();
  }


  private void Reset() {
    component ??= GetComponent<T>();
  }
}
