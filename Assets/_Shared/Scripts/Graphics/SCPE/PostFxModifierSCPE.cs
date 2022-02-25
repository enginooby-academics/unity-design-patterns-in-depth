#if ASSET_SCPE
using UnityEngine;
using UnityEngine.Rendering;

namespace Enginooby.Graphics {
  // ? Rename to PFX_SCPE
  // Collection of functions for modifying important parameters of Beautify FX setting
  // Usage: for short keys (controller) or UI
  // TODO: experiment inclusive/exclusive settings (settings which look good/bad when combine together)
  [RequireComponent(typeof(Volume))]
  public partial class PostFxModifierSCPE : MonoBehaviour {
    private VolumeProfile _profile;
    private Volume _volume;
    private string statesDebug; // OPTIM: use StringBuilder

    public Volume Volume => _volume ??= gameObject.GetComponent<Volume>();
    public VolumeProfile Profile => _profile ? _profile : Volume.profile;

    private void Awake() {
      _volume = gameObject.GetComponent<Volume>();
      GetScpeSettings();
      GetScpeOriginalStates();
    }

    public void Reset() {
      ResetStylizedSettings();
      ResetImageSettings();
      ResetBlurringSettings();
      ResetRenderingSettings();
      ResetRetroSettings();
      ResetScreenSettings();
    }

    private void GetScpeSettings() {
      GetStylizedSettings();
      GetImageSettings();
      GetEnvironmentSettings();
      GetBlurringSettings();
      GetRederingSettings();
      GetRetroSettings();
      GetScreenSettings();
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
}
#endif