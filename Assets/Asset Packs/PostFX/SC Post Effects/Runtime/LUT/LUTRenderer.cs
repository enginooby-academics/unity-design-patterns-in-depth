#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEngine.Rendering;
using UnityEngine;

namespace SCPE
{
#if URP
    public class LUTRenderer : ScriptableRendererFeature
    {
        class LUTRenderPass : PostEffectRenderer<LUT>
        {

            public LUTRenderPass(EffectBaseSettings settings)
            {
                this.settings = settings;
                shaderName = ShaderNames.LUT;
                ProfilerTag = this.ToString();
            }

            public void Setup(ScriptableRenderer renderer)
            {
                this.cameraColorTarget = GetCameraTarget(renderer);
                this.cameraDepthTarget = GetCameraDepthTarget(renderer);
                volumeSettings = VolumeManager.instance.stack.GetComponent<LUT>();
                
                if(volumeSettings && volumeSettings.IsActive()) renderer.EnqueuePass(this);
            }

            public override void ConfigurePass(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                if (!volumeSettings) return;

                requiresDepth = volumeSettings.mode == LUT.Mode.DistanceBased;

                base.ConfigurePass(cmd, cameraTextureDescriptor);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (ShouldRender(renderingData) == false) return;

                if (LUT.Bypass) return;

                var cmd = CommandBufferPool.Get(ProfilerTag);

                CopyTargets(cmd, renderingData);

                if (volumeSettings.lutNear.value)
                {
                    Material.SetTexture("_LUT_Near", volumeSettings.lutNear.value);
                    Material.SetVector("_LUT_Params", new Vector4(1f / volumeSettings.lutNear.value.width, 1f / volumeSettings.lutNear.value.height, volumeSettings.lutNear.value.height - 1f, volumeSettings.intensity.value));
                }

                if ((int)volumeSettings.mode.value == 1)
                {
                    Material.SetVector("_FadeParams", new Vector4(volumeSettings.startFadeDistance.value, volumeSettings.endFadeDistance.value, 0, 0));

                    if (volumeSettings.lutFar.value) Material.SetTexture("_LUT_Far", volumeSettings.lutFar.value);
                }

                FinalBlit(this, context, cmd, renderingData, mainTexHandle.id, cameraColorTarget, Material, (int)volumeSettings.mode.value);
            }
        }

        LUTRenderPass m_ScriptablePass;

        [SerializeField]
        public EffectBaseSettings settings = new EffectBaseSettings();

        public override void Create()
        {
            m_ScriptablePass = new LUTRenderPass(settings);
            m_ScriptablePass.renderPassEvent = settings.injectionPoint;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            m_ScriptablePass.Setup(renderer);
        }
    }
#endif
}
