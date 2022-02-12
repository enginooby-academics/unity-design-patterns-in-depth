#if ASSET_SCPE
using SCPE;
using UnityEngine;

// using QFSW.QC;

namespace Enginoobz.Graphics {
  public partial class PostFxModifierSCPE : MonoBehaviour {
    private Caustics caustics;
    private bool causticsOriginalState;
    private CloudShadows cloudShadows;
    private bool cloudShadowsOriginalState;
    private Fog fog;
    private bool fogOriginalState;

    private void GetEnviromentOriginalStates() {
      cloudShadowsOriginalState = cloudShadows.active;
      causticsOriginalState = caustics.active;
      fogOriginalState = fog.active;
    }

    private void GetEnvironmentSettings() {
      Profile.TryGet(out cloudShadows);
      Profile.TryGet(out caustics);
      Profile.TryGet(out fog);
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
#endif