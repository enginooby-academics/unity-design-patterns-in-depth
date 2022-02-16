using Enginooby.Graphics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FacadePattern.Case1.Base {
  /// <summary>
  ///   The 'Facade' class. It is setup in the lazy manner, only init/load resources when client first request servivce.
  ///   Makes use of many related 3rd-party tools as subsystems together to change the overall graphics.
  /// </summary>
  public class GraphicsController : MonoBehaviourSingleton<GraphicsController> {
    public void SetupHorror() {
      _postFXSytem.LoadVolumeProfile("Graphics/Profiles/PFX_Horror3");
#if ASSET_SCPE && ASSET_BEAUTIFY
      _scpeSystem.ActivateFog(true);
      _scpeSystem.ActivateBlackBars(false);
      _scpeSystem.ActivateKuwahara(false);
      _scpeSystem.ActivateDithering(false);
      _scpeSystem.ActivatePosterize(false);
      _beautifySystem.ActivateVignette(true);
      _beautifySystem.ActivateOutline(true);
#endif
    }

    public void SetupCinematic() {
      _postFXSytem.LoadVolumeProfile("Graphics/Profiles/PFX_Movie4");
#if ASSET_SCPE && ASSET_BEAUTIFY
      _scpeSystem.ActivateFog(false);
      _scpeSystem.ActivateBlackBars(true);
      _scpeSystem.ActivateKuwahara(false);
      _scpeSystem.ActivateDithering(false);
      _scpeSystem.ActivatePosterize(false);
      _beautifySystem.ActivateVignette(false);
      _beautifySystem.ActivateOutline(false);
#endif
    }

    public void SetupArtistic() {
      _postFXSytem.LoadVolumeProfile("Graphics/Profiles/PFX_Coziness2");
#if ASSET_SCPE && ASSET_BEAUTIFY
      _scpeSystem.ActivateKuwahara(true);
      _scpeSystem.ActivateDithering(true);
      _scpeSystem.ActivatePosterize(true);
      _scpeSystem.ActivateFog(false);
      _scpeSystem.ActivateBlackBars(false);
      _beautifySystem.ActivateVignette(false);
      _beautifySystem.ActivateOutline(false);
#endif
    }

    #region SUBSYSTEMS ===================================================================================================================================

#if ASSET_SCPE && ASSET_BEAUTIFY
    private PostFxModifierSCPE _scpeSystem;
    private PostFxModifierBeautify _beautifySystem;
#endif
    private Volume _postFXSytem;

    private void Awake() {
      // SetupCamera();
      InitBeautifySystem();
      InitSCPESystem();
      InitPostFXSystem();
    }

    private void SetupCamera() {
      var uac = Camera.main.GetComponent<UniversalAdditionalCameraData>();
      uac.renderPostProcessing = true;
      uac.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
      uac.antialiasingQuality = AntialiasingQuality.High;
    }

    // FIX: only init Beautify successfully after each recompiling
    private void InitBeautifySystem() {
#if ASSET_SCPE && ASSET_BEAUTIFY
      var beautifyGameObject = new GameObject("Beautify");
      beautifyGameObject.HideInHierarchy();
      var beautifyVolume = beautifyGameObject.AddComponent<Volume>();
      beautifyVolume.LoadVolumeProfile("Graphics/PFX_Beautify_FacadePattern");
      // beautifyGameObject.AddComponent<Beautify.Universal.BeautifySettings>();
      _beautifySystem = beautifyGameObject.AddComponent<PostFxModifierBeautify>();
#endif
    }

    private void InitSCPESystem() {
#if ASSET_SCPE && ASSET_BEAUTIFY
      var scpeGameObject = new GameObject("SCPE");
      scpeGameObject.HideInHierarchy();
      var scpeVolume = scpeGameObject.AddComponent<Volume>();
      scpeVolume.LoadVolumeProfile("Graphics/PFX_SCPE_FacadePattern");
      _scpeSystem = scpeGameObject.AddComponent<PostFxModifierSCPE>();
#endif
    }

    private void InitPostFXSystem() {
      var postFXGameObject = new GameObject("PostFX");
      postFXGameObject.HideInHierarchy();
      _postFXSytem = postFXGameObject.AddComponent<Volume>();
    }

    private void OnDestroy() {
#if ASSET_SCPE && ASSET_BEAUTIFY
      _beautifySystem.DestroyGameObject();
      _scpeSystem.DestroyGameObject();
#endif
      _postFXSytem.DestroyGameObject();
    }

    // TIP: use same label for #endregion to keep "=" equal

    #endregion SUBSYSTEMS ================================================================================================================================
  }
}