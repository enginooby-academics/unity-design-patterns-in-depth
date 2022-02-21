using UnityEngine;

/// <summary>
///   Extend MonoBehaviourOperator if T is MonoBehaviour.
/// </summary>
public abstract class ComponentOperator<T> : MonoBehaviourBase where T : Component {
  // Behaviour
  [SerializeField] protected T _component;

  // public virtual void Awake(){
  //  Reset();
  // }

  // ! ??= operator does not work
  protected virtual void Reset() {
    if (!_component) _component = gameObject.TryAddComponent<T>();
  }
}

public abstract class MonoBehaviourOperator<T> : ComponentOperator<T> where T : MonoBehaviour {
  public void DisableComponent() => _component.enabled = false;
}