using UnityEngine;

namespace MediatorPattern.Case1.Base {
  /// <summary>
  /// Manage and switch between mediators.
  /// </summary>
  public class CollisionResolverManager : MonoBehaviourSingleton<CollisionResolverManager> {
    [SerializeReference]
    private ICollisionResolver _currentResolver;
    public ICollisionResolver CurrentResolver => _currentResolver;

    public void SwitchResolver(ICollisionResolver resolver) {
      _currentResolver = resolver;
    }
  }
}
