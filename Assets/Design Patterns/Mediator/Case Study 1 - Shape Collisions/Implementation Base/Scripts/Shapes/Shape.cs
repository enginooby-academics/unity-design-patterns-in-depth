using UnityEngine;

namespace MediatorPattern.Case1.Base {
  /// <summary>
  /// * The 'Abstract Colleague' class
  /// </summary>
  public abstract class Shape : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
      CollisionResolverManager.Instance.CurrentResolver.ResolveCollision(this, other.GetComponent<Shape>());
    }
  }
}
