using UnityEngine;
using Beautify.Universal;
using UnityEngine.Rendering;
using QFSW.QC;

// Dependencies: Beautify 2
// Collection of functions for modifying important parameters of Beautify FX setting
// Used for short keys (controller) or UI

[RequireComponent(typeof(Volume))]
public class PostFxModifierBeautify : MonoBehaviour {
  private Beautify.Universal.Beautify settings;
  private bool originalBlurState;
  private bool originalSharpenState;
  private bool originalVignettingOuterRingState;
  private bool originalVignettingInnerRingState;
  private bool originalOutlineState;
  private bool originalNightVisionState;

  private void Start() {
    GetBeautifySettings();
  }

  private void GetBeautifySettings() {
    settings = BeautifySettings.settings;
    originalBlurState = settings.blurIntensity.overrideState;
    originalSharpenState = settings.sharpenIntensity.overrideState;
    originalVignettingOuterRingState = settings.vignettingOuterRing.overrideState;
    originalVignettingInnerRingState = settings.vignettingInnerRing.overrideState;
    originalNightVisionState = settings.nightVision.overrideState;
    originalOutlineState = settings.outline.overrideState;
  }

  public void Disable() {
    settings.active = false;
  }

  public void ToggleCompareMode() {
    settings.active = true;
    settings.compareMode.overrideState = !settings.compareMode.overrideState;
  }

  [Command(CommandPrefix.PostFx + "blur-toggle")]
  public void ToggleBlur() {
    settings.blurIntensity.overrideState = !settings.blurIntensity.overrideState;
  }

  [Command(CommandPrefix.PostFx + "sharpen-toggle")]
  public void ToggleSharpen() {
    settings.sharpenIntensity.overrideState = !settings.sharpenIntensity.overrideState;
  }

  [Command(CommandPrefix.PostFx + "vignette-toggle")]
  public void ToggleVignette() {
    settings.vignettingOuterRing.overrideState = !settings.vignettingOuterRing.overrideState;
    settings.vignettingInnerRing.overrideState = !settings.vignettingInnerRing.overrideState;
  }

  [Command(CommandPrefix.PostFx + "night-vision-toggle")]
  public void ToggleNightVision() {
    settings.nightVision.overrideState = !settings.nightVision.overrideState;
  }

  [Command(CommandPrefix.PostFx + "outline-toggle")]
  public void ToggleOutline() {
    settings.outline.overrideState = !settings.outline.overrideState;
  }

  public void Reset() {
    settings.blurIntensity.overrideState = originalBlurState;
    settings.vignettingOuterRing.overrideState = originalVignettingOuterRingState;
    settings.vignettingInnerRing.overrideState = originalVignettingInnerRingState;
    settings.nightVision.overrideState = originalNightVisionState;
    settings.outline.overrideState = originalOutlineState;
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
