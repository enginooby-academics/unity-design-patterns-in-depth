using UnityEngine;

namespace MediatorPattern.Case1.Base {
  /// <summary>
  ///   Manage and switch between mediators.
  /// </summary>
  public class CollisionResolverManager : MonoBehaviourSingleton<CollisionResolverManager> {
    [field: SerializeReference] public ICollisionResolver CurrentResolver { get; private set; }

    public void SwitchResolver(ICollisionResolver resolver) {
      CurrentResolver = resolver;
    }
  }
}