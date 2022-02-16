#if ASSET_SCPE
using SCPE;
using UnityEngine;

// using QFSW.QC;

namespace Enginooby.Graphics {
  public partial class PostFxModifierSCPE : MonoBehaviour {
    private ColorSplit colorSplit;

    private bool colorSplitOriginalState;
    private Dithering dithering;
    private bool ditheringOriginalState;
    private Pixelize pixelize;
    private bool pixelizeOriginalState;
    private Posterize posterize;
    private bool posterizeOriginalState;
    private Scanlines scanlines;
    private bool scanlinesOriginalState;

    private void GetRetroOriginalStates() {
      colorSplitOriginalState = colorSplit.active;
      ditheringOriginalState = dithering.active;
      pixelizeOriginalState = pixelize.active;
      posterizeOriginalState = posterize.active;
      scanlinesOriginalState = scanlines.active;
    }

    private void ResetRetroSettings() {
      colorSplit.active = colorSplitOriginalState;
      dithering.active = ditheringOriginalState;
      pixelize.active = pixelizeOriginalState;
      posterize.active = posterizeOriginalState;
      scanlines.active = scanlinesOriginalState;
    }

    private void RandomizeRetroSettings() {
      colorSplit.SetActiveOnRandom(.2f);
      dithering.SetActiveOnRandom();
      pixelize.SetActiveOnRandom(.2f);
      posterize.SetActiveOnRandom();
      scanlines.SetActiveOnRandom();
      if (colorSplit.active) statesDebug += "colorSplit ";
      if (dithering.active) statesDebug += "dithering ";
      if (pixelize.active) statesDebug += "pixelize ";
      if (posterize.active) statesDebug += "posterize ";
      if (scanlines.active) statesDebug += "scanlines ";
    }

    private void GetRetroSettings() {
      Profile.TryGet(out colorSplit);
      Profile.TryGet(out dithering);
      Profile.TryGet(out pixelize);
      Profile.TryGet(out posterize);
      Profile.TryGet(out scanlines);
    }

    // [Command(CommandPrefix.PostFx + "color-split-toggle")]
    public void ToggleColorSplit() {
      colorSplit.active = !colorSplit.active;
    }

    // [Command(CommandPrefix.PostFx + "dithering-toggle")]
    public void ToggleDithering() {
      dithering.active = !dithering.active;
    }

    public void ActivateDithering(bool isActive) {
      dithering.active = isActive;
    }

    // [Command(CommandPrefix.PostFx + "pixelize-toggle")]
    public void TogglePixelize() {
      pixelize.active = !pixelize.active;
    }

    // [Command(CommandPrefix.PostFx + "posterize-toggle")]
    public void TogglePosterize() {
      posterize.active = !posterize.active;
    }

    public void ActivatePosterize(bool isActive) {
      posterize.active = isActive;
    }

    // [Command(CommandPrefix.PostFx + "scanlines-toggle")]
    public void ToggleScanlines() {
      scanlines.active = !scanlines.active;
    }
  }
}
#endif