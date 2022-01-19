using UnityEngine;
using QFSW.QC;
using Sirenix.OdinInspector;

namespace Enginoobz.Graphics {
  public class PostFxModifierController : MonoBehaviour {
    // TODO: Separate all keys into SO
    // [SerializeField] private KeyCodeModifier modifierKey = new KeyCodeModifier(keyCode: KeyCode.LeftShift);
    [SerializeField] private KeyCodeModifier randomizeKey = new KeyCodeModifier(keyCode: KeyCode.LeftBracket);
    [SerializeField] private KeyCodeModifier resetKey = new KeyCodeModifier(keyCode: KeyCode.RightBracket);

    #region BEAUTIFY
    [Header("[Beautify]")]
    [SerializeField] private KeyCodeModifier disableBeautifyKey = new KeyCodeModifier(keyCode: KeyCode.U);
    [SerializeField] private KeyCodeModifier toggleCompareModeKey = new KeyCodeModifier(keyCode: KeyCode.I);
    [SerializeField] private KeyCodeModifier toggleBlurKey = new KeyCodeModifier(keyCode: KeyCode.Alpha6);
    [SerializeField] private KeyCodeModifier toggleSharpenKey = new KeyCodeModifier(keyCode: KeyCode.Alpha7);
    [SerializeField] private KeyCodeModifier toggleVignetteKey = new KeyCodeModifier(keyCode: KeyCode.Alpha8);
    [SerializeField] private KeyCodeModifier toggleNightVisionKey = new KeyCodeModifier(keyCode: KeyCode.Alpha9);
    [SerializeField] private KeyCodeModifier toggleOutlineKey = new KeyCodeModifier(keyCode: KeyCode.Alpha0);

    [OnValueChanged(nameof(UpdateGlobalBeautifyModifierKey))]
    [SerializeField] [EnumToggleButtons] ModifierKey globalBeautifyModifierKey;
    private void UpdateGlobalBeautifyModifierKey() {
      ModifierKey commonModifierKey = globalBeautifyModifierKey;
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

    [Header("[Stylized]")]
    [SerializeField] private KeyCodeModifier toggleSketchKey = new KeyCodeModifier(keyCode: KeyCode.Alpha1);
    [SerializeField] private KeyCodeModifier toggleKuwaharaKey = new KeyCodeModifier(keyCode: KeyCode.Alpha2);
    [SerializeField] private KeyCodeModifier toggleEdgeDetectionKey = new KeyCodeModifier(keyCode: KeyCode.Alpha3);
    [SerializeField] private KeyCodeModifier toggleMosaicKey = new KeyCodeModifier(keyCode: KeyCode.None);

    [Header("[Image]")]
    [SerializeField] private KeyCodeModifier toggleHueShift3dKey = new KeyCodeModifier(keyCode: KeyCode.Alpha4);
    [SerializeField] private KeyCodeModifier toggleColorizeKey = new KeyCodeModifier(keyCode: KeyCode.Alpha5);

    [Header("[Blurring]")]
    [SerializeField] private KeyCodeModifier doubleVisionKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier tiltShiftKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier radialBlurKey = new KeyCodeModifier(keyCode: KeyCode.None);

    [Header("[Rendering]")]
    [SerializeField] private KeyCodeModifier lensFlaresKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier lightStreaksKey = new KeyCodeModifier(keyCode: KeyCode.None);

    [Header("[Retro]")]
    [SerializeField] private KeyCodeModifier colorSplitKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier ditheringKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier pixelizeKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier posterizeKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier scanlinesKey = new KeyCodeModifier(keyCode: KeyCode.None);

    [Header("[Retro]")]
    [SerializeField] private KeyCodeModifier blackBarsKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier dangerKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier gradientKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier refractionKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier ripplesKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier speedLinesKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier tubeDistortionKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier kaleidoscopeKey = new KeyCodeModifier(keyCode: KeyCode.None);

    [Header("[Environment]")]
    [SerializeField] private KeyCodeModifier toggleCloudShadowsKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier toggleCausticsKey = new KeyCodeModifier(keyCode: KeyCode.None);
    [SerializeField] private KeyCodeModifier toggleFogKey = new KeyCodeModifier(keyCode: KeyCode.None);

    private PostFxModifierSCPE scpeModifier;

    void Start() {
      beautifyModifier = FindObjectOfType<PostFxModifierBeautify>();
      scpeModifier = FindObjectOfType<PostFxModifierSCPE>();
    }

    void Update() {
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

    [Command(CommandPrefix.PostFx + "random")]
    private void Randomize() {
      beautifyModifier.Randomize();
      scpeModifier.Randomize();
    }

    [Command(CommandPrefix.PostFx + "reset")]
    private void Reset() {
      beautifyModifier.Reset();
      scpeModifier.Reset();
    }
  }
}
