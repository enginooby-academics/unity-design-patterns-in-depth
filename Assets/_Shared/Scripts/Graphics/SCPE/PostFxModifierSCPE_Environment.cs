using UnityEngine;
using SCPE;
// using QFSW.QC;

namespace Enginoobz.Graphics {
  public partial class PostFxModifierSCPE : MonoBehaviour {
    private CloudShadows cloudShadows;
    private Caustics caustics;
    private Fog fog;

    private bool cloudShadowsOriginalState;
    private bool causticsOriginalState;
    private bool fogOriginalState;

    private void GetEnviromentOriginalStates() {
      cloudShadowsOriginalState = cloudShadows.active;
      causticsOriginalState = caustics.active;
      fogOriginalState = fog.active;
    }

    private void GetEnvironmentSettings() {
      Profile.TryGet<CloudShadows>(out cloudShadows);
      Profile.TryGet<Caustics>(out caustics);
      Profile.TryGet<Fog>(out fog);
    }

    // [Command(CommandPrefix.Environment + "cloud-shadows-toggle")]
    public void ToggleCloudShadows() {
      cloudShadows.active = !cloudShadows.active;
    }

    // [Command(CommandPrefix.Environment + "caustics-toggle")]
    public void ToggleCaustics() {
      caustics.active = !caustics.active;
    }

    // [Command(CommandPrefix.Environment + "fog-toggle")]
    public void ToggleFog() {
      fog.active = !fog.active;
    }

    public void ActivateFog(bool isActive) {
      fog.active = isActive;
    }
  }
}