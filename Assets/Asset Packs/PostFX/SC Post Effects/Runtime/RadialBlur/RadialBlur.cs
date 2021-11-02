#if URP
using UnityEngine.Rendering.Universal;
#endif

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace SCPE
{
#if URP
    [Serializable, VolumeComponentMenu("SC Post Effects/Blurring/Radial Blur")]
    public sealed class RadialBlur : VolumeComponent, IPostProcessComponent
    {
        [Range(0f, 1f)]
        public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);

        [Range(3f, 12f)]
        public ClampedIntParameter iterations = new ClampedIntParameter(6, 3,12);

        public bool IsActive() => amount.value > 0f && this.active;

        public bool IsTileCompatible() => false;
    }
#endif
}