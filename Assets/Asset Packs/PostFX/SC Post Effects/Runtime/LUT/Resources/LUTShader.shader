Shader "Hidden/SC Post Effects/LUT"
{
	HLSLINCLUDE

	#define REQUIRE_DEPTH
	#include "../../../Shaders/Pipeline/Pipeline.hlsl"

	DECLARE_TEX(_LUT_Near);
	DECLARE_TEX(_LUT_Far);
	SamplerState sampler_LUT_Near;
	SamplerState sampler_LUT_Far;

	float4 _LUT_Params;
	float4 _FadeParams;
	float _Invert;

	//Note: When the GLES/OpenCL API is used TEXTURE2D_ARGS does not accept a sampler parameter
    //2.1.1, macros handle this now, safe to pass in a samplerstate

	float3 FormatStrip(half3 uvw, half3 scaleOffset)
	{
		//Strip format where `height = sqrt(width)`
		uvw.z *= scaleOffset.z;
		half shift = floor(uvw.z);
		uvw.xy = uvw.xy * scaleOffset.z * scaleOffset.xy + scaleOffset.xy * 0.5;
		uvw.x += shift * scaleOffset.y;

		uvw.z -= shift;

		return uvw;
	}

	half3 ApplyLut2d(TEX_ARG(tex, samplerTex), half3 uvw, half3 scaleOffset)
	{
		uvw = FormatStrip(uvw, scaleOffset);

		float3 centerLut = SAMPLE_TEX(tex, samplerTex, uvw.xy).rgb;
		float3 offsetLut = SAMPLE_TEX(tex, samplerTex, uvw.xy + half2(scaleOffset.y, 0)).rgb;

		uvw.xyz = lerp(centerLut, offsetLut, uvw.z);

		return uvw;
	}
    
    inline float3 Grade(TEX_ARG(lut, samplerTex), half3 rgb) 
	{
		half3 colorGraded;

		rgb = saturate(rgb);
        
        #if !UNITY_COLORSPACE_GAMMA //Linear
        rgb = LinearToSRGB(rgb);
        #endif

        colorGraded = ApplyLut2d(TEX_PARAM(lut, samplerTex), rgb, _LUT_Params.xyz);

#if !UNITY_COLORSPACE_GAMMA //Linear
        colorGraded = SRGBToLinear(colorGraded);
#endif//End linear

		return colorGraded;
	}

	float4 FragSingle(Varyings i) : SV_Target
	{
		STEREO_EYE_INDEX_POST_VERTEX(i);

		float4 screenColor = ScreenColor(UV_VR);
		screenColor = lerp(screenColor, 1 - screenColor, _Invert);

		half3 colorGraded = Grade(TEX_PARAM(_LUT_Near, sampler_LUT_Near), screenColor.rgb);

		float3 color = lerp(screenColor.rgb, colorGraded.rgb, _LUT_Params.w);


		return float4(color.rgb, screenColor.a);
	}

	float4 FragDuo(Varyings i) : SV_Target
	{
		STEREO_EYE_INDEX_POST_VERTEX(i);

		float4 screenColor = ScreenColor(UV_VR);
		screenColor = lerp(screenColor, 1 - screenColor, _Invert);

		float depth = SAMPLE_DEPTH(UV_VR);

		half3 gradedNear = Grade(TEX_PARAM(_LUT_Near, sampler_LUT_Near), screenColor.rgb);
		half3 gradedFar = Grade(TEX_PARAM(_LUT_Far, sampler_LUT_Far), screenColor.rgb);

		float fadeDist = LinearDepthFade(LINEAR_DEPTH(depth), _FadeParams.x, _FadeParams.y, 0.0, 1.0);
		float3 color = lerp(gradedFar, gradedNear, fadeDist);

		color = lerp(color, 1 - color, _Invert);

		return float4(lerp(screenColor.rgb, color.rgb, _LUT_Params.w), screenColor.a);
	}

	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			Name "Color Grading LUT"
			HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment FragSingle

			ENDHLSL
		}
		Pass //Depth based
		{
			Name "Dual Color Grading LUT"
			HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment FragDuo

			ENDHLSL
		}
	}
}