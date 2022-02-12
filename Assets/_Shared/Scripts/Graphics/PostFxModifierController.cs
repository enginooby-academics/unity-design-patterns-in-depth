#if ASSET_SCPE && ASSET_BEAUTIFY
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using UnityEngine;

#else
using Enginoobz.Attribute;
#endif

// using QFSW.QC;

namespace Enginoobz.Graphics {
  public class PostFxModifierController : MonoBehaviour {
    // TODO: Separate all keys into SO
    // [SerializeField] private KeyCodeModifier modifierKey = new KeyCodeModifier(keyCode: KeyCode.LeftShift);
    [SerializeField] private KeyCodeModifier randomizeKey = new KeyCodeModifier(KeyCode.LeftBracket);
    [SerializeField] private KeyCodeModifier resetKey = new KeyCodeModifier(KeyCode.RightBracket);

    [Header("[Stylized]")] [SerializeField]
    private KeyCodeModifier toggleSketchKey = new KeyCodeModifier(KeyCode.Alpha1);

    [SerializeField] private KeyCodeModifier toggleKuwaharaKey = new KeyCodeModifier(KeyCode.Alpha2);
    [SerializeField] private KeyCodeModifier toggleEdgeDetectionKey = new KeyCodeModifier(KeyCode.Alpha3);
    [SerializeField] private KeyCodeModifier toggleMosaicKey = new KeyCodeModifier();

    [Header("[Image]")] [SerializeField]
    private KeyCodeModifier toggleHueShift3dKey = new KeyCodeModifier(KeyCode.Alpha4);

    [SerializeField] private KeyCodeModifier toggleColorizeKey = new KeyCodeModifier(KeyCode.Alpha5);

    [Header("[Blurring]")] [SerializeField]
    private KeyCodeModifier doubleVisionKey = new KeyCodeModifier();

    [SerializeField] private KeyCodeModifier tiltShiftKey = new KeyCodeModifier();
    [SerializeField] private KeyCodeModifier radialBlurKey = new KeyCodeModifier();

    [Header("[Rendering]")] [SerializeField]
    private KeyCodeModifier lensFlaresKey = new KeyCodeModifier();

    [SerializeField] private KeyCodeModifier lightStreaksKey = new KeyCodeModifier();

    [Header("[Retro]")] [SerializeField] private KeyCodeModifier colorSplitKey = new KeyCodeModifier();

    [SerializeField] private KeyCodeModifier ditheringKey = new KeyCodeModifier();
    [SerializeField] private KeyCodeModifier pixelizeKey = new KeyCodeModifier();
    [SerializeField] private KeyCodeModifier posterizeKey = new KeyCodeModifier();
    [SerializeField] private KeyCodeModifier scanlinesKey = new KeyCodeModifier();

    [Header("[Retro]")] [SerializeField] private KeyCodeModifier blackBarsKey = new KeyCodeModifier();

    [SerializeField] private KeyCodeModifier dangerKey = new KeyCodeModifier();
    [SerializeField] private KeyCodeModifier gradientKey = new KeyCodeModifier();
    [SerializeField] private KeyCodeModifier refractionKey = new KeyCodeModifier();
    [SerializeField] private KeyCodeModifier ripplesKey = new KeyCodeModifier();
    [SerializeField] private KeyCodeModifier speedLinesKey = new KeyCodeModifier();
    [SerializeField] private KeyCodeModifier tubeDistortionKey = new KeyCodeModifier();
    [SerializeField] private KeyCodeModifier kaleidoscopeKey = new KeyCodeModifier();

    [Header("[Environment]")] [SerializeField]
    private KeyCodeModifier toggleCloudShadowsKey = new KeyCodeModifier();

    [SerializeField] private KeyCodeModifier toggleCausticsKey = new KeyCodeModifier();
    [SerializeField] private KeyCodeModifier toggleFogKey = new KeyCodeModifier();

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
    private KeyCodeModifier disableBeautifyKey = new KeyCodeModifier(KeyCode.U);

    [SerializeField] private KeyCodeModifier toggleCompareModeKey = new KeyCodeModifier(KeyCode.I);
    [SerializeField] private KeyCodeModifier toggleBlurKey = new KeyCodeModifier(KeyCode.Alpha6);
    [SerializeField] private KeyCodeModifier toggleSharpenKey = new KeyCodeModifier(KeyCode.Alpha7);
    [SerializeField] private KeyCodeModifier toggleVignetteKey = new KeyCodeModifier(KeyCode.Alpha8);
    [SerializeField] private KeyCodeModifier toggleNightVisionKey = new KeyCodeModifier(KeyCode.Alpha9);
    [SerializeField] private KeyCodeModifier toggleOutlineKey = new KeyCodeModifier(KeyCode.Alpha0);

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