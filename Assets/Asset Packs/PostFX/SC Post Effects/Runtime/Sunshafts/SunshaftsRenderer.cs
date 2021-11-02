#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEngine;
using UnityEngine.Rendering;

namespace SCPE
{
#if URP
    public class SunshaftsRenderer : ScriptableRendererFeature
    {
        class SunshaftsRenderPass : PostEffectRenderer<Sunshafts>
        {
            private int skyboxBufferID;
            int blurredID;
            int blurredID2;
            private Vector2Int resolution;
            private int downsampling;

            public SunshaftsRenderPass(EffectBaseSettings settings)
            {
                this.settings = settings;
                shaderName = ShaderNames.Sunshafts;
                requiresDepth = true;
                ProfilerTag = this.ToString();
                
                skyboxBufferID = Shader.PropertyToID("_SkyboxBuffer");
                blurredID = Shader.PropertyToID("_Temp1");
                blurredID2 = Shader.PropertyToID("_Temp2");
            }

            public void Setup(ScriptableRenderer renderer)
            {
                this.cameraColorTarget = GetCameraTarget(renderer);
                this.cameraDepthTarget = GetCameraDepthTarget(renderer);
                volumeSettings = VolumeManager.instance.stack.GetComponent<Sunshafts>();
                
                if(volumeSettings && volumeSettings.IsActive()) renderer.EnqueuePass(this);
            }

            public override void ConfigurePass(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                if (!volumeSettings) return;
                
                downsampling = (int)volumeSettings.resolution.value;

                RenderTextureDescriptor opaqueDesc = cameraTextureDescriptor;
                opaqueDesc.depthBufferBits = 0;
                resolution = new Vector2Int(opaqueDesc.width, opaqueDesc.height);
                opaqueDesc.width /= 2;
                opaqueDesc.height /= 2;
                cmd.GetTemporaryRT(skyboxBufferID, opaqueDesc);

                opaqueDesc.width = cameraTextureDescriptor.width / (int)volumeSettings.resolution.value;
                opaqueDesc.height = cameraTextureDescriptor.height / (int)volumeSettings.resolution.value;
                opaqueDesc.msaaSamples = 1;

                cmd.GetTemporaryRT(blurredID, opaqueDesc);
                cmd.GetTemporaryRT(blurredID2, opaqueDesc);
                
                base.ConfigurePass(cmd, cameraTextureDescriptor);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (ShouldRender(renderingData) == false) return;
                
                base.Execute(context, ref renderingData);

                var cmd = CommandBufferPool.Get(ProfilerTag);
                
                int res = (int)volumeSettings.resolution.value;

                //If resolution changed, recreate buffers
                if(res != downsampling)
                {
                    cmd.ReleaseTemporaryRT(blurredID);
                    cmd.ReleaseTemporaryRT(blurredID2);

                    cmd.GetTemporaryRT(blurredID, resolution.x / res, resolution.y / res, 0, FilterMode.Bilinear);
                    cmd.GetTemporaryRT(blurredID2, resolution.x / res, resolution.y / res, 0, FilterMode.Bilinear);

                    cmd.ReleaseTemporaryRT(mainTexHandle.id);
                    cmd.GetTemporaryRT(mainTexHandle.id, resolution.x / res, resolution.y / res, 0, FilterMode.Bilinear);

                    Debug.Log("Recreated blur buffers");
                    downsampling = res;
                }
                
                CopyTargets(cmd, renderingData);

                #region Parameters
                float sunIntensity = (volumeSettings.useCasterIntensity.value) ? SunshaftCaster.intensity : volumeSettings.sunShaftIntensity.value;

                //Screen-space sun position
                Vector3 v = Vector3.one * 0.5f;
                if (Sunshafts.sunPosition != Vector3.zero)
                    v = renderingData.cameraData.camera.WorldToViewportPoint(Sunshafts.sunPosition);
                else
                    v = new Vector3(0.5f, 0.5f, 0.0f);
                Material.SetVector("_SunPosition", new Vector4(v.x, v.y, sunIntensity, volumeSettings.falloff.value));

                Color col = (volumeSettings.useCasterColor.value) ? SunshaftCaster.color : volumeSettings.sunColor.value;
                Material.SetFloat("_BlendMode", (int)volumeSettings.blendMode.value);
                Material.SetColor("_SunColor", (v.z >= 0.0f) ? col : new Color(0, 0, 0, 0));
                Material.SetColor("_SunThreshold", volumeSettings.sunThreshold.value);
                #endregion
                
                #region Blur
                cmd.BeginSample("Sunshafts blur");
                
                Blit(this, cmd, mainTexHandle.id, skyboxBufferID, Material, (int)SunshaftsBase.Pass.SkySource);
                Blit(cmd, skyboxBufferID, blurredID);

                float offset = volumeSettings.length.value * (1.0f / 768.0f);

                int iterations = (volumeSettings.highQuality.value) ? 2 : 1;
                float blurAmount = (volumeSettings.highQuality.value) ? volumeSettings.length.value / 2.5f : volumeSettings.length.value;

                for (int i = 0; i < iterations; i++)
                {
                    Blit(this, cmd, blurredID, blurredID2, Material, (int)SunshaftsBase.Pass.RadialBlur);
                    offset = blurAmount * (((i * 2.0f + 1.0f) * 6.0f)) / renderingData.cameraData.camera.pixelWidth;
                    Material.SetFloat("_BlurRadius", offset);

                    Blit(this, cmd, blurredID2, blurredID, Material, (int)SunshaftsBase.Pass.RadialBlur);
                    offset = blurAmount * (((i * 2.0f + 1.0f) * 6.0f)) / renderingData.cameraData.camera.pixelHeight;
                    Material.SetFloat("_BlurRadius", offset);

                }
                cmd.EndSample("Sunshafts blur");

                #endregion

                cmd.SetGlobalTexture("_SunshaftBuffer", blurredID);

                FinalBlit(this, context, cmd, renderingData, mainTexHandle.id, cameraColorTarget, Material, (int)SunshaftsBase.Pass.Blend);
            }

            public override void Cleanup(CommandBuffer cmd)
            {
                cmd.ReleaseTemporaryRT(skyboxBufferID);
                cmd.ReleaseTemporaryRT(blurredID);
                cmd.ReleaseTemporaryRT(blurredID2);
                
                base.Cleanup(cmd);
            }
        }

        SunshaftsRenderPass m_ScriptablePass;

        [SerializeField]
        public EffectBaseSettings settings = new EffectBaseSettings();

        public override void Create()
        {
            m_ScriptablePass = new SunshaftsRenderPass(settings);
            m_ScriptablePass.renderPassEvent = settings.injectionPoint;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            m_ScriptablePass.Setup(renderer);
        }
    }
#endif
}
