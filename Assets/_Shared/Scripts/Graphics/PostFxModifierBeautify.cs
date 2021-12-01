using UnityEngine;
using Beautify.Universal;
using UnityEngine.Rendering;
using QFSW.QC;

namespace Enginoobz.Graphics {
  // Collection of functions for modifying important parameters of Beautify FX setting
  // Used for short keys (controller) or UI
  [RequireComponent(typeof(Volume))]
  public class PostFxModifierBeautify : MonoBehaviour {
    private Beautify.Universal.Beautify _settings;
    public Beautify.Universal.Beautify Settings => _settings ?? BeautifySettings.settings;

    private bool originalBlurState;
    private bool originalSharpenState;
    private bool originalVignettingOuterRingState;
    private bool originalVignettingInnerRingState;
    private bool originalOutlineState;
    private bool originalNightVisionState;

    private void Awake() {
      GetBeautifySettings();
    }

    private void GetBeautifySettings() {
      originalBlurState = Settings.blurIntensity.overrideState;
      originalSharpenState = Settings.sharpenIntensity.overrideState;
      originalVignettingOuterRingState = Settings.vignettingOuterRing.overrideState;
      originalVignettingInnerRingState = Settings.vignettingInnerRing.overrideState;
      originalNightVisionState = Settings.nightVision.overrideState;
      originalOutlineState = Settings.outline.overrideState;
    }

    public void Disable() {
      Settings.active = false;
    }

    public void ToggleCompareMode() {
      Settings.active = true;
      Settings.compareMode.overrideState = !Settings.compareMode.overrideState;
    }

    [Command(CommandPrefix.PostFx + "blur-toggle")]
    public void ToggleBlur() {
      Settings.blurIntensity.overrideState = !Settings.blurIntensity.overrideState;
    }

    [Command(CommandPrefix.PostFx + "sharpen-toggle")]
    public void ToggleSharpen() {
      Settings.sharpenIntensity.overrideState = !Settings.sharpenIntensity.overrideState;
    }

    [Command(CommandPrefix.PostFx + "vignette-toggle")]
    public void ToggleVignette() {
      Settings.vignettingOuterRing.overrideState = !Settings.vignettingOuterRing.overrideState;
      Settings.vignettingInnerRing.overrideState = !Settings.vignettingInnerRing.overrideState;
    }

    public void ActivateVignette(bool isActive) {
      Settings.vignettingOuterRing.overrideState = isActive;
      Settings.vignettingInnerRing.overrideState = isActive;
    }

    [Command(CommandPrefix.PostFx + "night-vision-toggle")]
    public void ToggleNightVision() {
      Settings.nightVision.overrideState = !Settings.nightVision.overrideState;
    }

    [Command(CommandPrefix.PostFx + "outline-toggle")]
    public void ToggleOutline() {
      Settings.outline.overrideState = !Settings.outline.overrideState;
    }

    public void ActivateOutline(bool isActive) {
      Settings.outline.overrideState = isActive;
    }

    public void Reset() {
      Settings.blurIntensity.overrideState = originalBlurState;
      Settings.vignettingOuterRing.overrideState = originalVignettingOuterRingState;
      Settings.vignettingInnerRing.overrideState = originalVignettingInnerRingState;
      Settings.nightVision.overrideState = originalNightVisionState;
      Settings.outline.overrideState = originalOutlineState;
    }

    public void Randomize() {
      if (Random.value < .5f) ToggleBlur();
      if (Random.value < .5f) ToggleBlur();
      if (Random.value < .5f) ToggleOutline();
      if (Random.value < .5f) ToggleVignette();
      if (Random.value < .2f) ToggleNightVision();
      if (Random.value < .5f) ToggleSharpen();

    }
  }
}
