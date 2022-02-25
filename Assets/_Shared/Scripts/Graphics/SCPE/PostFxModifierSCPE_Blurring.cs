#if ASSET_SCPE
using SCPE;
using UnityEngine;

// using QFSW.QC;

namespace Enginooby.Graphics {
  public partial class PostFxModifierSCPE : MonoBehaviour {
    private DoubleVision _doubleVision;
    private bool _doubleVisionOriginalState;
    private RadialBlur _radialBlur;
    private bool _radialBlurOriginalState;
    private TiltShift _tiltShift;
    private bool _tiltShiftOriginalState;

    private void GetBlurringOriginalStates() {
      _doubleVisionOriginalState = _doubleVision.active;
      _tiltShiftOriginalState = _tiltShift.active;
      _radialBlurOriginalState = _radialBlur.active;
    }

    private void ResetBlurringSettings() {
      _doubleVision.active = _doubleVisionOriginalState;
      _tiltShift.active = _tiltShiftOriginalState;
      _radialBlur.active = _radialBlurOriginalState;
    }

    private void RandomizeBlurringSettings() {
      _doubleVision.SetActiveOnRandom();
      _tiltShift.SetActiveOnRandom();
      _radialBlur.SetActiveOnRandom();
      if (_doubleVision.active) statesDebug += "doubleVision ";
      if (_tiltShift.active) statesDebug += "tiltShift ";
      if (_radialBlur.active) statesDebug += "radialBlur ";
    }

    private void GetBlurringSettings() {
      Profile.TryGet(out _doubleVision);
      Profile.TryGet(out _tiltShift);
      Profile.TryGet(out _radialBlur);
    }

    // [Command(CommandPrefix.PostFx + "double-vision-toggle")]
    public void ToggleDoubleVision() {
      _doubleVision.active = !_doubleVision.active;
    }

    // [Command(CommandPrefix.PostFx + "tilt-shift-toggle")]
    public void ToggleTiltShift() => _tiltShift.ToggleActive();

    // [Command(CommandPrefix.PostFx + "blur-radial-toggle")]
    public void ToggleRadialBlur() => _radialBlur.ToggleActive();
  }
}
#endif