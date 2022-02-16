using UnityEngine;

// TODO: Apply for MonoBehaviourBase in the Library

namespace UpdateMethodPattern.Case1.Unity2 {
  /// <summary>
  /// Base class for objects with a custom Update method. <br/>
  /// Override OnStart & OnUpdate instead of using Start & Update.
  /// </summary>
  public class RegisteredUpdateBehaviour : MonoBehaviour, IUpdatable {
    private void Start() {
      UpdateManager.Instance.RegisterUpdating(this);
      OnStart();
    }

    // This is a custom Start method which the child can override,
    // for convenience: we don't want to override Start and invoke base.Start() in every child,
    // for contract: if we omit base.Start(), the object is not registered.
    protected virtual void OnStart() { }

    // Custom update method, which the child can override
    public virtual void OnUpdate(float deltaTime) { }

    // Remember that it's dangerous to call another method in OnDestroy()
    // because the other method might already be destroyed
    private void OnDestroy() => UpdateManager.Instance.UnregisterUpdating(this);
  }
}