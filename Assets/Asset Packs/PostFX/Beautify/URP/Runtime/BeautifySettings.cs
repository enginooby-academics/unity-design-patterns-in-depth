using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Beautify.Universal {

    public delegate float OnBeforeFocusEvent(float currentFocusDistance);


    [ExecuteInEditMode]
    public class BeautifySettings : MonoBehaviour {

        [Header("Scene Settings")]
        public Transform sun;
        public Transform depthOfFieldTarget;

        public OnBeforeFocusEvent OnBeforeFocus;

        [NonSerialized]
        public static float depthOfFieldCurrentFocalPointDistance;

        [NonSerialized]
        public static int bloomExcludeMask;

        [NonSerialized]
        public static bool dofTransparentSupport;

        [NonSerialized]
        public static int dofTransparentLayerMask;

        static BeautifySettings _instance;
        static Volume _beautifyVolume;
        static Beautify _beautify;

        /// <summary>
        /// Returns a reference to the Beautify Settings component attached to the Post Processing Layer or camera
        /// </summary>
        /// <value>The instance.</value>
        public static BeautifySettings instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<BeautifySettings>();
                    if (_instance == null) {
                        // Check if there's a single volume component, then add BeautifySettings singleton to that gameobject
                        // otherwise create a new dedicated gameobject
                        _beautifyVolume = FindBeautifyVolume();
                        GameObject go;
                        if (_beautifyVolume == null) {
                            go = new GameObject("Beautify Scene Settings", typeof(BeautifySettings));
                        } else {
                            go = _beautifyVolume.gameObject;
                            _instance = go.GetComponent<BeautifySettings>();
                        }
                        if (_instance == null) {
                            _instance = go.AddComponent<BeautifySettings>();
                        }
                    }
                }
                return _instance;
            }
        }


        static Volume FindBeautifyVolume() {
            Volume[] vols = FindObjectsOfType<Volume>();
            foreach (Volume volume in vols) {
                if (volume.sharedProfile != null && volume.sharedProfile.Has<Beautify>()) {
                    _beautifyVolume = volume;
                    return volume;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a reference to the settings of Beautify in the Post Processing Profile
        /// </summary>
        /// <value>The shared settings.</value>
        public static Beautify sharedSettings {
            get {
                if (_beautify != null) return _beautify;
                if (_beautifyVolume == null) FindBeautifyVolume();
                if (_beautifyVolume == null) return null;

                bool foundEffectSettings = _beautifyVolume.sharedProfile.TryGet(out _beautify);
                if (!foundEffectSettings) {
                    Debug.Log("Cant load Beautify settings");
                    return null;
                }
                return _beautify;
            }
        }

        /// <summary>
        /// Returns a copy of the settings of Beautify in the Post Processing Profile
        /// </summary>
        /// <value>The settings.</value>
        public static Beautify settings {
            get {
                if (_beautify != null) return _beautify;
                if (_beautifyVolume == null) FindBeautifyVolume();
                if (_beautifyVolume == null) return null;

                bool foundEffectSettings = _beautifyVolume.profile.TryGet(out _beautify);
                if (!foundEffectSettings) {
                    Debug.Log("Cant load Beautify settings");
                    return null;
                }
                return _beautify;
            }
        }


        public static void Blink(float duration, float maxValue = 1) {
            if (duration <= 0)
                return;

            instance.StartCoroutine(instance.DoBlink(duration, maxValue));
        }

        IEnumerator DoBlink(float duration, float maxValue) {

            Beautify beautify = settings;
            float start = Time.time;
            WaitForEndOfFrame w = new WaitForEndOfFrame();
            beautify.vignettingBlink.overrideState = true;
            float t;
            // Close
            do {
                t = (Time.time - start) / duration;
                if (t > 1f)
                    t = 1f;
                float easeOut = t * (2f - t);
                beautify.vignettingBlink.value = easeOut * maxValue;
                yield return w;
            } while (t < 1f);

            // Open
            start = Time.time;
            do {
                t = (Time.time - start) / duration;
                if (t > 1f)
                    t = 1f;
                float easeIn = t * t;
                beautify.vignettingBlink.value = (1f - easeIn) * maxValue;
                yield return w;
            } while (t < 1f);
            beautify.vignettingBlink.overrideState = false;
        }

#if UNITY_EDITOR
        private void OnEnable() {
            ManageBuildOptimizationStatus();
        }

        static bool wasBuildOptActive;
        public static void ManageBuildOptimizationStatus() {
            Beautify beautify = sharedSettings;
            if (beautify == null) return;

            if (!beautify.active && wasBuildOptActive) {
                StripBeautifyKeywords();
            } else if (beautify.active && !wasBuildOptActive) {
                SetStripShaderKeywords(beautify);
            }
            wasBuildOptActive = beautify.active;
        }

        const string PLAYER_PREF_KEYNAME = "BeautifyStripKeywordSet";


        public static void StripBeautifyKeywords() {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(BeautifyRendererFeature.SKW_BLOOM);
            sb.Append(BeautifyRendererFeature.SKW_BLOOM_USE_DEPTH);
            sb.Append(BeautifyRendererFeature.SKW_DEPTH_OF_FIELD);
            sb.Append(BeautifyRendererFeature.SKW_DIRT);
            sb.Append(BeautifyRendererFeature.SKW_LUT);
            sb.Append(BeautifyRendererFeature.SKW_OUTLINE);
            sb.Append(BeautifyRendererFeature.SKW_NIGHT_VISION);
            sb.Append(BeautifyRendererFeature.SKW_PURKINJE);
            sb.Append(BeautifyRendererFeature.SKW_TONEMAP_ACES);
            sb.Append(BeautifyRendererFeature.SKW_VIGNETTING);
            sb.Append(BeautifyRendererFeature.SKW_VIGNETTING_MASK);
            sb.Append(BeautifyRendererFeature.SKW_COLOR_TWEAKS);
            sb.Append(BeautifyRendererFeature.SKW_TURBO);
            sb.Append(BeautifyRendererFeature.SKW_DITHER);
            sb.Append(BeautifyRendererFeature.SKW_SHARPEN);
            sb.Append(BeautifyRendererFeature.SKW_EYE_ADAPTATION);
            PlayerPrefs.SetString(PLAYER_PREF_KEYNAME, sb.ToString());
        }

        public static void SetStripShaderKeywords(Beautify beautify) {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            bool auto = beautify.optimizeBuildBeautifyAuto.value;
            if (auto) {
                beautify.stripBeautifyBloom.value = false;
                beautify.stripBeautifyDoF.value = false;
                beautify.stripBeautifyDoFTransparentSupport.value = false;
                beautify.stripBeautifyLensDirt.value = false;
                beautify.stripBeautifyLUT.value = false;
                beautify.stripBeautifyOutline.value = false;
                beautify.stripBeautifyNightVision.value = false;
                beautify.stripBeautifyColorTweaks.value = false;
                beautify.stripBeautifyPurkinje.value = false;
                beautify.stripBeautifyTonemapping.value = false;
                beautify.stripBeautifyDithering.value = false;
                beautify.stripBeautifySharpen.value = false;
                beautify.stripBeautifyEyeAdaptation.value = false;
                beautify.stripBeautifyVignetting.value = false;
            }
            if (beautify.stripBeautifyBloom.value || (auto && beautify.bloomIntensity.value <= 0)) {
                sb.Append(BeautifyRendererFeature.SKW_BLOOM);
                sb.Append(BeautifyRendererFeature.SKW_BLOOM_USE_DEPTH);
            }
            if (beautify.stripBeautifyDoF.value || (auto && !beautify.depthOfField.value)) {
                sb.Append(BeautifyRendererFeature.SKW_DEPTH_OF_FIELD);
            }
            if (beautify.stripBeautifyDoF.value || (auto && !beautify.depthOfFieldTransparentSupport.value)) {
                sb.Append(BeautifyRendererFeature.SKW_DEPTH_OF_FIELD_TRANSPARENT);
            }
            if (beautify.stripBeautifyLensDirt.value || (auto && beautify.lensDirtIntensity.value <= 0)) {
                sb.Append(BeautifyRendererFeature.SKW_DIRT);
            }
            bool usesLUT = beautify.lut.value && beautify.lutTexture.value != null;
            if (beautify.stripBeautifyLUT.value || (auto && !usesLUT)) {
                sb.Append(BeautifyRendererFeature.SKW_LUT);
            }
            if (beautify.stripBeautifyOutline.value || (auto && !beautify.outline.value)) {
                sb.Append(BeautifyRendererFeature.SKW_OUTLINE);
            }
            if (beautify.stripBeautifyNightVision.value || (auto && !beautify.nightVision.value)) {
                sb.Append(BeautifyRendererFeature.SKW_NIGHT_VISION);
            }
            bool usesColorTweaks = beautify.sepia.value > 0 || beautify.daltonize.value > 0 || beautify.colorTempBlend.value > 0;
            if (beautify.stripBeautifyColorTweaks.value || (auto && !usesColorTweaks)) {
                sb.Append(BeautifyRendererFeature.SKW_COLOR_TWEAKS);
            }
            if (beautify.stripBeautifyPurkinje.value || (auto && !beautify.purkinje.value)) {
                sb.Append(BeautifyRendererFeature.SKW_PURKINJE);
            }
            if (beautify.stripBeautifyTonemapping.value || (auto && beautify.tonemap.value != Beautify.TonemapOperator.ACES)) {
                sb.Append(BeautifyRendererFeature.SKW_TONEMAP_ACES);
            }
            if (beautify.stripBeautifyDithering.value || (auto && beautify.ditherIntensity.value <= 0)) {
                sb.Append(BeautifyRendererFeature.SKW_DITHER);
            }
            if (beautify.stripBeautifySharpen.value || (auto && beautify.sharpenIntensity.value <= 0)) {
                sb.Append(BeautifyRendererFeature.SKW_SHARPEN);
            }
            if (beautify.stripBeautifyEyeAdaptation.value || (auto && !beautify.eyeAdaptation.value)) {
                sb.Append(BeautifyRendererFeature.SKW_EYE_ADAPTATION);
            }

            float outerRing = 1f - beautify.vignettingOuterRing.value;
            float innerRing = 1f - beautify.vignettingInnerRing.value;
            bool vignettingEnabled = outerRing < 1 || innerRing < 1f || beautify.vignettingFade.value > 0 || beautify.vignettingBlink.value > 0;
            if (beautify.stripBeautifyVignetting.value || (auto && !vignettingEnabled)) {
                sb.Append(BeautifyRendererFeature.SKW_VIGNETTING);
                sb.Append(BeautifyRendererFeature.SKW_VIGNETTING_MASK);
            }

            bool stripUnityPPS = beautify.optimizeBuildUnityPPSAuto.value;
            if (beautify.stripUnityBloom.value || stripUnityPPS) {
                sb.Append("_BLOOM_LQ _BLOOM_HQ _BLOOM_LQ_DIRT _BLOOM_HQ_DIRT");
            }
            if (beautify.stripUnityChromaticAberration.value || stripUnityPPS) {
                sb.Append("_CHROMATIC_ABERRATION");
            }
            if (beautify.stripUnityDistortion.value || stripUnityPPS) {
                sb.Append("_DISTORTION");
            }
            if (beautify.stripUnityFilmGrain.value || stripUnityPPS) {
                sb.Append("_FILM_GRAIN");
            }
            if (beautify.stripUnityTonemapping.value || stripUnityPPS) {
                sb.Append("_TONEMAP_ACES");
            }
            PlayerPrefs.SetString(PLAYER_PREF_KEYNAME, sb.ToString());
        }
#endif
    }
}
