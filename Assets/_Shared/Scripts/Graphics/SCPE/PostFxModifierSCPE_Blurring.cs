using UnityEngine;
using SCPE;
// using QFSW.QC;

namespace Enginoobz.Graphics {
  public partial class PostFxModifierSCPE : MonoBehaviour {
    private DoubleVision doubleVision;
    private TiltShift tiltShift;
    private RadialBlur radialBlur;

    private bool doubleVisionOriginalState;
    private bool tiltShiftOriginalState;
    private bool radialBlurOriginalState;

    private void GetBlurringOriginalStates() {
      doubleVisionOriginalState = doubleVision.active;
      tiltShiftOriginalState = tiltShift.active;
      radialBlurOriginalState = radialBlur.active;
    }

    private void ResetBlurringSettings() {
      doubleVision.active = doubleVisionOriginalState;
      tiltShift.active = tiltShiftOriginalState;
      radialBlur.active = radialBlurOriginalState;
    }

    private void RandomizeBlurringSettings() {
      doubleVision.SetActiveOnRandom();
      tiltShift.SetActiveOnRandom();
      radialBlur.SetActiveOnRandom();
      if (doubleVision.active) statesDebug += "doubleVision ";
      if (tiltShift.active) statesDebug += "tiltShift ";
      if (radialBlur.active) statesDebug += "radialBlur ";
    }

    private void GetBlurringSettings() {
      Profile.TryGet<DoubleVision>(out doubleVision);
      Profile.TryGet<TiltShift>(out tiltShift);
      Profile.TryGet<RadialBlur>(out radialBlur);
    }

    // [Command(CommandPrefix.PostFx + "double-vision-toggle")]
    public void ToggleDoubleVision() {
      doubleVision.active = !doubleVision.active;
    }

    // [Command(CommandPrefix.PostFx + "tilt-shift-toggle")]
    public void ToggleTiltShift() {
      tiltShift.active = !tiltShift.active;
    }

    // [Command(CommandPrefix.PostFx + "blur-radial-toggle")]
    public void ToggleRadialBlur() {
      radialBlur.active = !radialBlur.active;
    }
  }
}