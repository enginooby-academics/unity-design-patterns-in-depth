#if URP
using UnityEngine.Rendering.Universal;
#endif

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace SCPE
{
#if URP
    [Serializable, VolumeComponentMenu("SC Post Effects/Retro/Pixelize")]
    public sealed class Pixelize : VolumeComponent, IPostProcessComponent
    {
        public ClampedFloatParameter amount = new ClampedFloatParameter(0f, 0f, 1f);

        public bool IsActive() => amount.value > 0f && this.active;

        public bool IsTileCompatible() => false;
    }
#endif
}