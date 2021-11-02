using System;
using UnityEngine;

#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEngine.Rendering;

namespace SCPE
{
#if URP
    [Serializable, VolumeComponentMenu("SC Post Effects/Image/Sharpen")]
    public sealed class Sharpen : VolumeComponent, IPostProcessComponent
    {
        public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);
        public ClampedFloatParameter radius = new ClampedFloatParameter(1f, 0.1f, 2f);

        public bool IsActive() { return active && amount.value > 0f; }

        public bool IsTileCompatible() => false;
    }
#endif
}