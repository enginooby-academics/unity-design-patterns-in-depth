using System;
using UnityEngine;

#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEngine.Rendering;

namespace SCPE
{
#if URP
    [Serializable, VolumeComponentMenu("SC Post Effects/Image/Color Grading LUT")]
    public sealed class LUT : VolumeComponent, IPostProcessComponent
    {
        public enum Mode
        {
            Single = 0,
            DistanceBased = 1,
        }

        [Serializable]
        public sealed class ModeParam : VolumeParameter<Mode> { }

        [Tooltip("Distance-based mode blends two LUTs over a distance")]
        public ModeParam mode = new ModeParam { value = Mode.Single };

        public FloatParameter startFadeDistance = new FloatParameter(0f);
        public FloatParameter endFadeDistance = new FloatParameter(1000f);

        [Range(0f, 1f), Tooltip("Fades the effect in or out")]
        public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

        [Tooltip("Supply a LUT strip texture.")]
        public TextureParameter lutNear = new TextureParameter(null);
        // [DisplayName("Far")]
        public TextureParameter lutFar = new TextureParameter(null);

        public bool IsActive() { return active && intensity.value > 0f; }

        public bool IsTileCompatible() => false;

        //Used to temporarily disable the effect while capturing a screenshot for LUT extraction
        public static bool Bypass = false;
    }
#endif
}