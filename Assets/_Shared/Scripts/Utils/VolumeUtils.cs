using UnityEngine;
using UnityEngine.Rendering;
#if URP
using UnityEngine.Rendering.Universal;
#endif

public static class VolumeUtils {
  public static void ToggleState(this VolumeComponent volumeComponent) {
    volumeComponent.active = !volumeComponent.active;
  }

  /// <summary> Given a propability number from 0 to 1, if match set active, otherwise set inactive </summary>
  public static void SetActiveOnRandom(this VolumeComponent volumeComponent, float probability = .5f) {
    volumeComponent.active = Random.value < probability ? true : false;
  }

  /// <summary> Set the same state (active or inactive) for all components </summary>
  public static void SetActiveOnRandomInclusively(this VolumeComponent[] volumeComponents, float probability = .5f) {
    // IMPL
  }

  /// <summary> Set one component of the collection active randomly, set inactive for the rest </summary>
  public static void SetActiveOnRandomExclusively(this VolumeComponent[] volumeComponents) {
    // IMPL
  }
}