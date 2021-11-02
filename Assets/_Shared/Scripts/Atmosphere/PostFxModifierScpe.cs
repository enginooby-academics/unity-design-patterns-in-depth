// Dependencies: SC Post Effects

using UnityEngine;
using UnityEngine.Rendering;
using SCPE;
using QFSW.QC;

// Collection of functions for modifying important parameters of Beautify FX setting
// Usage: for short keys (controller) or UI
// TODO: experiment inclusive/exclusive settings (settings which look good/bad when combine together)
[RequireComponent(typeof(Volume))]
public class PostFxModifierScpe : MonoBehaviour {
  private Volume volume;
  private string statesDebug;

  #region STYLIZED
  private Sketch sketch;
  private Kuwahara kuwahara;
  private EdgeDetection edgeDetection;
  private Mosaic mosaic;
  private bool sketchOriginalState;
  private bool kuwaharaOriginalState;
  private bool edgeDetectionOriginalState;
  private bool mosaicOriginalState;
  private void GetStylizedSettings() {
    volume.profile.TryGet<Sketch>(out sketch);
    volume.profile.TryGet<Kuwahara>(out kuwahara);
    volume.profile.TryGet<EdgeDetection>(out edgeDetection);
    volume.profile.TryGet<Mosaic>(out mosaic);
  }
  private void GetStylizedOriginalStates() {
    sketchOriginalState = sketch.active;
    kuwaharaOriginalState = kuwahara.active;
    edgeDetectionOriginalState = edgeDetection.active;
    mosaicOriginalState = mosaic.active;
  }
  private void ResetStylizedSettings() {
    sketch.active = sketchOriginalState;
    kuwahara.active = kuwaharaOriginalState;
    edgeDetection.active = edgeDetectionOriginalState;
    mosaic.active = mosaicOriginalState;
  }
  private void RandomizeStylizedSettings() {
    sketch.SetActiveOnRandom();
    kuwahara.SetActiveOnRandom();
    edgeDetection.SetActiveOnRandom();
    mosaic.SetActiveOnRandom();
    if (sketch.active) statesDebug += "sketch ";
    if (kuwahara.active) statesDebug += "kuwahara ";
    if (edgeDetection.active) statesDebug += "edgeDetection ";
    if (mosaic.active) statesDebug += "mosaic ";
  }

  [Command(CommandPrefix.PostFx + "sketch-toggle")]
  public void ToggleSketch() {
    sketch.active = !sketch.active;
  }

  [Command(CommandPrefix.PostFx + "kuwahara-toggle")]
  public void ToggleKuwahara() {
    kuwahara.active = !kuwahara.active;
  }

  [Command(CommandPrefix.PostFx + "edge-detection-toggle")]
  public void ToggleEdgeDetection() {
    edgeDetection.active = !edgeDetection.active;
  }

  [Command(CommandPrefix.PostFx + "mosaic-toggle")]
  public void ToggleMosaic() {
    mosaic.active = !mosaic.active;
  }
  #endregion

  #region IMAGE
  private Colorize colorize;
  private HueShift3D hueShift3D;
  private bool colorizeOriginalState;
  private bool hueShift3DOriginalState;
  private void GetImageOriginalStates() {
    colorizeOriginalState = colorize.active;
    hueShift3DOriginalState = hueShift3D.active;
  }
  private void ResetImageSettings() {
    colorize.active = colorizeOriginalState;
    hueShift3D.active = hueShift3DOriginalState;
  }
  private void RandomizeImageSettings() {
    colorize.SetActiveOnRandom(.2f);
    hueShift3D.SetActiveOnRandom(.2f);
    if (colorize.active) statesDebug += "colorize ";
    if (hueShift3D.active) statesDebug += "hueShift3D ";
  }
  private void GetImageSettings() {
    volume.profile.TryGet<Colorize>(out colorize);
    volume.profile.TryGet<HueShift3D>(out hueShift3D);
  }

  [Command(CommandPrefix.PostFx + "colorize-toggle")]
  public void ToggleColorize() {
    colorize.active = !colorize.active;
  }

  [Command(CommandPrefix.PostFx + "hue-shift-toggle")]
  public void ToggleHueShift3D() {
    hueShift3D.active = !hueShift3D.active;
  }
  #endregion

  #region ENVIRONMENT
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
    volume.profile.TryGet<CloudShadows>(out cloudShadows);
    volume.profile.TryGet<Caustics>(out caustics);
    volume.profile.TryGet<Fog>(out fog);
  }

  [Command(CommandPrefix.Environment + "cloud-shadows-toggle")]
  public void ToggleCloudShadows() {
    cloudShadows.active = !cloudShadows.active;
  }

  [Command(CommandPrefix.Environment + "caustics-toggle")]
  public void ToggleCaustics() {
    caustics.active = !caustics.active;
  }

  [Command(CommandPrefix.Environment + "fog-toggle")]
  public void ToggleFog() {
    fog.active = !fog.active;
  }
  #endregion

  #region BLURRING
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
    volume.profile.TryGet<DoubleVision>(out doubleVision);
    volume.profile.TryGet<TiltShift>(out tiltShift);
    volume.profile.TryGet<RadialBlur>(out radialBlur);
  }

  [Command(CommandPrefix.PostFx + "double-vision-toggle")]
  public void ToggleDoubleVision() {
    doubleVision.active = !doubleVision.active;
  }

  [Command(CommandPrefix.PostFx + "tilt-shift-toggle")]
  public void ToggleTiltShift() {
    tiltShift.active = !tiltShift.active;
  }

  [Command(CommandPrefix.PostFx + "blur-radial-toggle")]
  public void ToggleRadialBlur() {
    radialBlur.active = !radialBlur.active;
  }
  #endregion

  #region RENDERING
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
    volume.profile.TryGet<LensFlares>(out lensFlares);
    volume.profile.TryGet<LightStreaks>(out lightStreaks);
  }

  [Command(CommandPrefix.PostFx + "len-flares-toggle")]
  public void ToggleLenFlares() {
    lensFlares.active = !lensFlares.active;
  }

  [Command(CommandPrefix.PostFx + "light-streaks-toggle")]
  public void ToggleLightStreaks() {
    lightStreaks.active = !lightStreaks.active;
  }
  #endregion

  #region RETRO
  private ColorSplit colorSplit;
  private Dithering dithering;
  private Pixelize pixelize;
  private Posterize posterize;
  private Scanlines scanlines;
  private bool colorSplitOriginalState;
  private bool ditheringOriginalState;
  private bool pixelizeOriginalState;
  private bool posterizeOriginalState;
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
    volume.profile.TryGet<ColorSplit>(out colorSplit);
    volume.profile.TryGet<Dithering>(out dithering);
    volume.profile.TryGet<Pixelize>(out pixelize);
    volume.profile.TryGet<Posterize>(out posterize);
    volume.profile.TryGet<Scanlines>(out scanlines);
  }

  [Command(CommandPrefix.PostFx + "color-split-toggle")]
  public void ToggleColorSplit() {
    colorSplit.active = !colorSplit.active;
  }

  [Command(CommandPrefix.PostFx + "dithering-toggle")]
  public void ToggleDithering() {
    dithering.active = !dithering.active;
  }

  [Command(CommandPrefix.PostFx + "pixelize-toggle")]
  public void TogglePixelize() {
    pixelize.active = !pixelize.active;
  }

  [Command(CommandPrefix.PostFx + "posterize-toggle")]
  public void TogglePosterize() {
    posterize.active = !posterize.active;
  }

  [Command(CommandPrefix.PostFx + "scanlines-toggle")]
  public void ToggleScanlines() {
    scanlines.active = !scanlines.active;
  }
  #endregion

  #region SCREEN
  private BlackBars blackBars;
  private Danger danger;
  private SCPE.Gradient gradient;
  private Refraction refraction;
  private Ripples ripples;
  private SpeedLines speedLines;
  private TubeDistortion tubeDistortion;
  private bool blackBarsOriginalState;
  private bool dangerOriginalState;
  private bool gradientOriginalState;
  private bool refractionOriginalState;
  private bool ripplesOriginalState;
  private bool speedLinesOriginalState;
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
    volume.profile.TryGet<BlackBars>(out blackBars);
    volume.profile.TryGet<Danger>(out danger);
    volume.profile.TryGet<SCPE.Gradient>(out gradient);
    volume.profile.TryGet<Refraction>(out refraction);
    volume.profile.TryGet<Ripples>(out ripples);
    volume.profile.TryGet<SpeedLines>(out speedLines);
    volume.profile.TryGet<TubeDistortion>(out tubeDistortion);
  }

  [Command(CommandPrefix.PostFx + "tube-distortion-toggle")]
  public void ToggleTubeDistortion() {
    tubeDistortion.active = !tubeDistortion.active;
  }

  [Command(CommandPrefix.PostFx + "speed-lines-toggle")]
  public void ToggleSpeedLines() {
    speedLines.active = !speedLines.active;
  }

  [Command(CommandPrefix.PostFx + "ripples-toggle")]
  public void ToggleRipples() {
    ripples.active = !ripples.active;
  }

  [Command(CommandPrefix.PostFx + "refraction-toggle")]
  public void ToggleRefraction() {
    refraction.active = !refraction.active;
  }

  [Command(CommandPrefix.PostFx + "gradient-toggle")]
  public void ToggleGradient() {
    gradient.active = !gradient.active;
  }

  [Command(CommandPrefix.PostFx + "danger-toggle")]
  public void ToggleDanger() {
    danger.active = !danger.active;
  }

  [Command(CommandPrefix.PostFx + "black-bars-toggle")]
  public void ToggleBlackBars() {
    blackBars.active = !blackBars.active;
  }
  #endregion

  private Kaleidoscope kaleidoscope;
  [Command(CommandPrefix.PostFx + "kaleidoscope-toggle")]
  public void ToggleKaleidoscope() {
    kaleidoscope.ToggleState();
  }

  void Start() {
    volume = gameObject.GetComponent<Volume>();
    GetScpeSettings();
    GetScpeOriginalStates();
  }
  private void GetScpeSettings() {
    GetStylizedSettings();
    GetImageSettings();
    GetEnvironmentSettings();
    GetBlurringSettings();
    GetRederingSettings();
    GetRetroSettings();
    GetScreenSettings();
    volume.profile.TryGet<Kaleidoscope>(out kaleidoscope);
  }

  private void GetScpeOriginalStates() {
    GetStylizedOriginalStates();
    GetImageOriginalStates();
    GetEnviromentOriginalStates();
    GetBlurringOriginalStates();
    GetRenderingOriginalStates();
    GetRenderingOriginalStates();
    GetRetroOriginalStates();
    GetScreenOriginalStates();
  }

  public void Reset() {
    ResetStylizedSettings();
    ResetImageSettings();
    ResetBlurringSettings();
    ResetRenderingSettings();
    ResetRetroSettings();
    ResetScreenSettings();
  }

  public void Randomize() {
    statesDebug = ">>> ";
    RandomizeStylizedSettings();
    RandomizeImageSettings();
    RandomizeBlurringSettings();
    RandomizeRenderingSettings();
    RandomizeRetroSettings();
    RandomizeScreenSettings();
    // TODO: colorize text by categorize
    // statesDebug = $"<b><color=orange>{statesDebug}</color></b>";
    print(statesDebug);
  }
}
