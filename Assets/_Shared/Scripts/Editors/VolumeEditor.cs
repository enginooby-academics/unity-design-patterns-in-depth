using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;

// TODO: Rename to VolumeProfileVariation extending SO
// + Move to Graphics
[ExecuteInEditMode]
[RequireComponent(typeof(Volume))]
public class VolumeEditor : MonoBehaviour {
  [SerializeField]
  private List<VolumeProfile> profiles;
  [ValueDropdown("profiles")]
  [SerializeField]
  private VolumeProfile currentProfile;

  private Volume volume;

  // Execute when Play & Stop 
  private void Start() {
    volume = GetComponent<Volume>();
    volume.profile = currentProfile;
    volume.weight = 0.05f;
  }

  // Execute when Recompile, Change serialized value, Play & Stop
  private void OnValidate() {
    if (volume is null) return;

    volume.profile = currentProfile;
  }

  public void SwitchNextVolume() {
    currentProfile = profiles.NavNext(currentProfile);
    volume.profile = currentProfile;
  }

  public void SwitchPreviousVolume() {
    currentProfile = profiles.NavPrevious(currentProfile);
    volume.profile = currentProfile;
  }

  public void IncreaseVolumeWeight() {
    if (volume != null & volume.weight < 1) volume.weight += 0.005f;
  }

  public void DecreaseVolumeWeight() {
    if (volume != null & volume.weight > 0) volume.weight -= 0.005f;
  }
}
