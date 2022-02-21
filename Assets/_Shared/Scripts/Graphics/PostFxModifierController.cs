#if ASSET_SCPE && ASSET_BEAUTIFY
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using UnityEngine;

#else
using Enginooby.Attribute;
#endif

// using QFSW.QC;

namespace Enginooby.Graphics {
  public class PostFxModifierController : MonoBehaviour {
    // TODO: Separate all keys into SO
    // [SerializeField] private KeyCodeModifier modifierKey = new KeyCodeModifier(keyCode: KeyCode.LeftShift);
    [SerializeField] private KeyCodeModifier randomizeKey = new(KeyCode.LeftBracket);
    [SerializeField] private KeyCodeModifier resetKey = new(KeyCode.RightBracket);

    [Header("[Stylized]")] [SerializeField]
    private KeyCodeModifier toggleSketchKey = new(KeyCode.Alpha1);

    [SerializeField] private KeyCodeModifier toggleKuwaharaKey = new(KeyCode.Alpha2);
    [SerializeField] private KeyCodeModifier toggleEdgeDetectionKey = new(KeyCode.Alpha3);
    [SerializeField] private KeyCodeModifier toggleMosaicKey = new();

    [Header("[Image]")] [SerializeField] private KeyCodeModifier toggleHueShift3dKey = new(KeyCode.Alpha4);

    [SerializeField] private KeyCodeModifier toggleColorizeKey = new(KeyCode.Alpha5);

    [Header("[Blurring]")] [SerializeField]
    private KeyCodeModifier doubleVisionKey = new();

    [SerializeField] private KeyCodeModifier tiltShiftKey = new();
    [SerializeField] private KeyCodeModifier radialBlurKey = new();

    [Header("[Rendering]")] [SerializeField]
    private KeyCodeModifier lensFlaresKey = new();

    [SerializeField] private KeyCodeModifier lightStreaksKey = new();

    [Header("[Retro]")] [SerializeField] private KeyCodeModifier colorSplitKey = new();

    [SerializeField] private KeyCodeModifier ditheringKey = new();
    [SerializeField] private KeyCodeModifier pixelizeKey = new();
    [SerializeField] private KeyCodeModifier posterizeKey = new();
    [SerializeField] private KeyCodeModifier scanlinesKey = new();

    [Header("[Retro]")] [SerializeField] private KeyCodeModifier blackBarsKey = new();

    [SerializeField] private KeyCodeModifier dangerKey = new();
    [SerializeField] private KeyCodeModifier gradientKey = new();
    [SerializeField] private KeyCodeModifier refractionKey = new();
    [SerializeField] private KeyCodeModifier ripplesKey = new();
    [SerializeField] private KeyCodeModifier speedLinesKey = new();
    [SerializeField] private KeyCodeModifier tubeDistortionKey = new();
    [SerializeField] private KeyCodeModifier kaleidoscopeKey = new();

    [Header("[Environment]")] [SerializeField]
    private KeyCodeModifier toggleCloudShadowsKey = new();

    [SerializeField] private KeyCodeModifier toggleCausticsKey = new();
    [SerializeField] private KeyCodeModifier toggleFogKey = new();

    private PostFxModifierSCPE scpeModifier;

    // [Command(CommandPrefix.PostFx + "reset")]
    private void Reset() {
      beautifyModifier.Reset();
      scpeModifier.Reset();
    }

    private void Start() {
      beautifyModifier = FindObjectOfType<PostFxModifierBeautify>();
      scpeModifier = FindObjectOfType<PostFxModifierSCPE>();
    }

    private void Update() {
      if (randomizeKey.IsTriggering) Randomize();
      if (resetKey.IsTriggering) Reset();

      if (toggleSketchKey.IsTriggering) scpeModifier.ToggleSketch();
      if (toggleKuwaharaKey.IsTriggering) scpeModifier.ToggleKuwahara();
      if (toggleEdgeDetectionKey.IsTriggering) scpeModifier.ToggleEdgeDetection();
      if (toggleMosaicKey.IsTriggering) scpeModifier.ToggleMosaic();

      if (toggleHueShift3dKey.IsTriggering) scpeModifier.ToggleHueShift3D();
      if (toggleColorizeKey.IsTriggering) scpeModifier.ToggleColorize();

      if (toggleCloudShadowsKey.IsTriggering) scpeModifier.ToggleCloudShadows();
      if (toggleCausticsKey.IsTriggering) scpeModifier.ToggleCaustics();
      if (toggleFogKey.IsTriggering) scpeModifier.ToggleFog();

      ProcessPostFxModifierBeautify();
    }

    private void OnDisable() {
      beautifyModifier.Reset();
    }

    // [Command(CommandPrefix.PostFx + "random")]
    private void Randomize() {
      beautifyModifier.Randomize();
      scpeModifier.Randomize();
    }

    #region BEAUTIFY

    [Header("[Beautify]")] [SerializeField]
    private KeyCodeModifier disableBeautifyKey = new(KeyCode.U);

    [SerializeField] private KeyCodeModifier toggleCompareModeKey = new(KeyCode.I);
    [SerializeField] private KeyCodeModifier toggleBlurKey = new(KeyCode.Alpha6);
    [SerializeField] private KeyCodeModifier toggleSharpenKey = new(KeyCode.Alpha7);
    [SerializeField] private KeyCodeModifier toggleVignetteKey = new(KeyCode.Alpha8);
    [SerializeField] private KeyCodeModifier toggleNightVisionKey = new(KeyCode.Alpha9);
    [SerializeField] private KeyCodeModifier toggleOutlineKey = new(KeyCode.Alpha0);

    [OnValueChanged(nameof(UpdateGlobalBeautifyModifierKey))] [SerializeField] [EnumToggleButtons]
    private ModifierKey globalBeautifyModifierKey;

    private void UpdateGlobalBeautifyModifierKey() {
      var commonModifierKey = globalBeautifyModifierKey;
      disableBeautifyKey.modifierKey = commonModifierKey;
      toggleCompareModeKey.modifierKey = commonModifierKey;
      toggleBlurKey.modifierKey = commonModifierKey;
      toggleSharpenKey.modifierKey = commonModifierKey;
      toggleVignetteKey.modifierKey = commonModifierKey;
      toggleNightVisionKey.modifierKey = commonModifierKey;
      toggleOutlineKey.modifierKey = commonModifierKey;
    }

    private PostFxModifierBeautify beautifyModifier;

    private void ProcessPostFxModifierBeautify() {
      if (disableBeautifyKey.IsTriggering) beautifyModifier.Disable();
      if (toggleCompareModeKey.IsTriggering) beautifyModifier.ToggleCompareMode();
      if (toggleBlurKey.IsTriggering) beautifyModifier.ToggleBlur();
      if (toggleSharpenKey.IsTriggering) beautifyModifier.ToggleSharpen();
      if (toggleVignetteKey.IsTriggering) beautifyModifier.ToggleVignette();
      if (toggleNightVisionKey.IsTriggering) beautifyModifier.ToggleNightVision();
      if (toggleOutlineKey.IsTriggering) beautifyModifier.ToggleOutline();
    }

    #endregion
  }
}
#endif