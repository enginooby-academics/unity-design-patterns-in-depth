using UnityEngine;

/// <summary>
///   GOs containing this type will be interacted when invoke InteractWithInteractables() of the Singleton. <br />
///   Other use case: provide custom interacting effect and custom trigger event for individual GO.
/// </summary>
public interface IInteractable<T> where T : GOI<T> {
  GameObject GameObject { get; }
  void OnInteracted();
}