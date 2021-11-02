using UnityEngine;
#if URP
using UnityEngine.Rendering.Universal;
#endif

namespace SCPE
{
    [System.Serializable]
    public class EffectBaseSettings
    {
#if URP
        public enum AllowedCameraType
        {
            Both,
            Overlay,
            Base,
        }

        [HideInInspector] //Can't execute after post-processing right now
        public RenderPassEvent injectionPoint = RenderPassEvent.BeforeRenderingPostProcessing;

        [Tooltip("Effect will render, even if the camera has post-processing disabled")]
        public bool alwaysEnable;

        [Tooltip("Effect also renders in the scene view, unless post-processing is disabled in the toolbar")]
        public bool allowInSceneView;

        [Tooltip("Configure which camera types the effect is allowed to render on when using camera stacking" +
                 "\n\nNote that some depth-based effects will not work with camera stacking, due to how the stacking system handles the depth texture")]
        public AllowedCameraType allowedCameraTypes = AllowedCameraType.Both;

        public EffectBaseSettings(bool allowInSceneView = true, AllowedCameraType allowedCameraTypes = AllowedCameraType.Both)
        {
            this.allowInSceneView = allowInSceneView;
            this.allowedCameraTypes = allowedCameraTypes;
        }
#endif
    }
}