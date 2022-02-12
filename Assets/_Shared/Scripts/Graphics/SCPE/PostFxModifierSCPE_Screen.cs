#if ASSET_SCPE
using SCPE;
using UnityEngine;
using Gradient = SCPE.Gradient;

// using QFSW.QC;

namespace Enginoobz.Graphics {
  public partial class PostFxModifierSCPE : MonoBehaviour {
    private BlackBars blackBars;

    private bool blackBarsOriginalState;
    private Danger danger;
    private bool dangerOriginalState;
    private Gradient gradient;
    private bool gradientOriginalState;
    private Kaleidoscope kaleidoscope;
    private Refraction refraction;
    private bool refractionOriginalState;
    private Ripples ripples;
    private bool ripplesOriginalState;
    private SpeedLines speedLines;
    private bool speedLinesOriginalState;
    private TubeDistortion tubeDistortion;
    private bool tubeDistortionOriginalState;

    private void GetScreenOriginalStates() {
      blackBarsOriginalState = blackBars.active;
      dangerOriginalState = danger.active;
      gradientOriginalState = gradient.active;
      refractionOriginalState = refraction.active;
      ripplesOriginalState = ripples.active;
      speedLinesOriginalState = speedLines.active;
      tubeDistortionOriginalState = tubeDistortion.active;
    }

    private void ResetScreenSettings() {
      blackBars.active = blackBarsOriginalState;
      danger.active = dangerOriginalState;
      gradient.active = gradientOriginalState;
      refraction.active = refractionOriginalState;
      ripples.active = ripplesOriginalState;
      speedLines.active = speedLinesOriginalState;
      tubeDistortion.active = tubeDistortionOriginalState;
    }

    private void RandomizeScreenSettings() {
      blackBars.SetActiveOnRandom();
      danger.SetActiveOnRandom(.2f);
      gradient.SetActiveOnRandom(.2f);
      refraction.SetActiveOnRandom();
      ripples.SetActiveOnRandom(.2f);
      speedLines.SetActiveOnRandom(.3f);
      tubeDistortion.SetActiveOnRandom();
      if (blackBars.active) statesDebug += "blackBars ";
      if (danger.active) statesDebug += "danger ";
      if (gradient.active) statesDebug += "gradient ";
      if (refraction.active) statesDebug += "refraction ";
      if (ripples.active) statesDebug += "ripples ";
      if (speedLines.active) statesDebug += "speedLines ";
      if (tubeDistortion.active) statesDebug += "tubeDistortion ";
    }

    private void GetScreenSettings() {
      Profile.TryGet(out blackBars);
      Profile.TryGet(out danger);
      Profile.TryGet(out gradient);
      Profile.TryGet(out refraction);
      Profile.TryGet(out ripples);
      Profile.TryGet(out speedLines);
      Profile.TryGet(out tubeDistortion);
      Profile.TryGet(out kaleidoscope);
    }

    // [Command(CommandPrefix.PostFx + "tube-distortion-toggle")]
    public void ToggleTubeDistortion() {
      tubeDistortion.active = !tubeDistortion.active;
    }

    // [Command(CommandPrefix.PostFx + "speed-lines-toggle")]
    public void ToggleSpeedLines() {
      speedLines.active = !speedLines.active;
    }

    // [Command(CommandPrefix.PostFx + "ripples-toggle")]
    public void ToggleRipples() {
      ripples.active = !ripples.active;
    }

    // [Command(CommandPrefix.PostFx + "refraction-toggle")]
    public void ToggleRefraction() {
      refraction.active = !refraction.active;
    }

    // [Command(CommandPrefix.PostFx + "gradient-toggle")]
    public void ToggleGradient() {
      gradient.active = !gradient.active;
    }

    // [Command(CommandPrefix.PostFx + "danger-toggle")]
    public void ToggleDanger() {
      danger.active = !danger.active;
    }

    // [Command(CommandPrefix.PostFx + "black-bars-toggle")]
    public void ToggleBlackBars() {
      blackBars.active = !blackBars.active;
    }

    public void ActivateBlackBars(bool isActive) {
      blackBars.active = isActive;
    }

    // [Command(CommandPrefix.PostFx + "kaleidoscope-toggle")]
    public void ToggleKaleidoscope() {
      kaleidoscope.ToggleState();
    }
  }
}
#endif