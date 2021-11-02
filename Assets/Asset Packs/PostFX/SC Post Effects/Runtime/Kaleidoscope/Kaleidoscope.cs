#if URP
using UnityEngine.Rendering.Universal;
#endif

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace SCPE
{
#if URP
    [Serializable, VolumeComponentMenu("SC Post Effects/Misc/Kaleidoscope")]
    public sealed class Kaleidoscope : VolumeComponent, IPostProcessComponent
    {
        [Range(0, 10), Tooltip("The number of times the screen is split up")]
        public ClampedIntParameter splits = new ClampedIntParameter(0, 0, 10);

        public bool IsActive() => splits.value > 0 && this.active;

        public bool IsTileCompatible() => false;
    }
#endif
}