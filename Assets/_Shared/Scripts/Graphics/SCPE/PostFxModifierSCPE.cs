// Dependencies: SC Post Effects
using UnityEngine;
using UnityEngine.Rendering;

namespace Enginoobz.Graphics {
  // Collection of functions for modifying important parameters of Beautify FX setting
  // Usage: for short keys (controller) or UI
  // TODO: experiment inclusive/exclusive settings (settings which look good/bad when combine together)
  [RequireComponent(typeof(Volume))]
  public partial class PostFxModifierSCPE : MonoBehaviour {
    private Volume _volume;
    private VolumeProfile _profile;
    private string statesDebug; // OPTIM: use StringBuilder

    public Volume Volume => _volume ??= gameObject.GetComponent<Volume>();
    public VolumeProfile Profile => _profile ?? Volume.profile;

    void Awake() {
      _volume = gameObject.GetComponent<Volume>();
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
}
