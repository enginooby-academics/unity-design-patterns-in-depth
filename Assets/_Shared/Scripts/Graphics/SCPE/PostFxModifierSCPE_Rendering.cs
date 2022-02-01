using UnityEngine;
using SCPE;
// using QFSW.QC;

namespace Enginoobz.Graphics {
  public partial class PostFxModifierSCPE : MonoBehaviour {
    private LensFlares lensFlares;
    private LightStreaks lightStreaks;

    private bool lensFlaresOriginalState;
    private bool lightStreaksOriginalState;

    private void GetRenderingOriginalStates() {
      lensFlaresOriginalState = lensFlares.active;
      lightStreaksOriginalState = lightStreaks.active;
    }

    private void ResetRenderingSettings() {
      lensFlares.active = lensFlaresOriginalState;
      lightStreaks.active = lightStreaksOriginalState;
    }

    private void RandomizeRenderingSettings() {
      lensFlares.SetActiveOnRandom();
      lightStreaks.SetActiveOnRandom();
      if (lensFlares.active) statesDebug += "lensFlares ";
      if (lightStreaks.active) statesDebug += "lightStreaks ";
    }

    private void GetRederingSettings() {
      Profile.TryGet<LensFlares>(out lensFlares);
      Profile.TryGet<LightStreaks>(out lightStreaks);
    }

    // [Command(CommandPrefix.PostFx + "len-flares-toggle")]
    public void ToggleLenFlares() {
      lensFlares.active = !lensFlares.active;
    }

    // [Command(CommandPrefix.PostFx + "light-streaks-toggle")]
    public void ToggleLightStreaks() {
      lightStreaks.active = !lightStreaks.active;
    }
  }
}