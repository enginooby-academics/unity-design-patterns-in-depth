Shader "Hidden/SC Post Effects/Fog"
{
	HLSLINCLUDE

	#define REQUIRE_DEPTH
	#include "../../../Shaders/Pipeline/Pipeline.hlsl"
	#include "../../../Shaders/Blurring.hlsl"
	#include "../Fog.hlsl"

	#pragma fragmentoption ARB_precision_hint_nicest

	uniform float4x4 clipToWorld;

	//Light scattering
	DECLARE_RT(_BloomTex);
	DECLARE_RT(_AutoExposureTex);

	float  _SampleScale;
	float4 _Threshold; // x: threshold value (linear), y: threshold - knee, z: knee * 2, w: 0.25 / knee
	float4 _ScatteringParams; // x: Sample scale y: Intensity z: 0 w: Itterations

	struct v2f {
		float4 positionCS : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 texcoordStereo : TEXCOORD1;
		float3 worldDirection : TEXCOORD2;
		float time : TEXCOORD3;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	v2f VertWorldSpaceReconstruction(Attributes v) {
		v2f o;

		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		o.positionCS = OBJECT_TO_CLIP(v);
		o.uv.xy = GET_TRIANGLE_UV(v);

		o.uv = FlipUV(o.uv);

		float4 clip = float4(o.uv.xy * 2 - 1, 0.0, 1.0);
		o.worldDirection = (mul((float4x4)clipToWorld, clip.rgba).xyz - _WorldSpaceCameraPos).xyz;
		o.time = _Time.y;

		//UNITY_SINGLE_PASS_STEREO
		o.texcoordStereo = GET_TRIANGLE_UV_VR(o.uv, v.vertexID);

		return o;
	}	

	half4 Prefilter(half4 color, float2 uv)
	{
		half autoExposure = SAMPLE_RT(_AutoExposureTex, Clamp, uv).r;
		color *= autoExposure;
		//color = min(_Params.x, color); // clamp to max
		color = QuadraticThreshold(color, _Threshold.x, _Threshold.yzw);
		return color;
	}

	half4 FragPrefilter(Varyings i) : SV_Target
	{
		half4 color = BoxFilter4(RT_PARAM(_MainTex, Clamp), UV, _MainTex_TexelSize.xy, 1);
		return Prefilter(SafeHDR(color), UV);
	}

		half4 FragDownsample(Varyings i) : SV_Target
	{
		half4 color = BoxFilter4(RT_PARAM(_MainTex, Clamp), UV, _MainTex_TexelSize.xy, 1);
		return color;
	}

		half4 Combine(half4 bloom, float2 uv)
	{
		half4 color = SAMPLE_RT(_BloomTex, Clamp, uv);
		return bloom + color;
	}

	half4 FragUpsample(Varyings i) : SV_Target
	{
		half4 bloom = UpsampleBox(RT_PARAM(_MainTex, Clamp), UV, _MainTex_TexelSize.xy, _SampleScale);
		return Combine(bloom, UV_VR);
	}

	float4 ComputeFog(float3 worldPos, float3 worldNormal, float2 screenPos, float depth, float time)
	{
		#if !defined(SHADERGRAPH_PREVIEW)
		float linearDepth = LINEAR_DEPTH(depth);

		float skyboxMask = 1;
		if (linearDepth > 0.99) skyboxMask = 0;

		//Same as GetFogDensity but distance/fog calculated separately
		float g = _DistanceParams.x;
		float distanceFog = GetFogDistance(worldPos, linearDepth);
		g += distanceFog;
		float heightFog = GetFogHeight(worldPos, time, skyboxMask);
		g += heightFog;
		float fogFactor = ComputeFactor(max(0.0, g));

		//Skybox influence
		if (linearDepth > 0.99) fogFactor = lerp(1.0, fogFactor, _SkyboxParams.x);

		//Color
		float4 fogColor = GetFogColor(screenPos, worldPos, distanceFog);

		fogColor.a = fogFactor;

		return fogColor;
		#else
		return 1.0
		#endif
	}

	float4 FragBlend(v2f i) : SV_Target
	{
		STEREO_EYE_INDEX_POST_VERTEX(i);

		half4 screenColor = ScreenColor(UV_VR);

		float depth = SAMPLE_DEPTH(UV_VR);
		float3 worldPos = i.worldDirection * LINEAR_EYE_DEPTH(depth) + _WorldSpaceCameraPos;
		
		//Alpha is density, do not modify
		float4 fogColor = ComputeFog(worldPos, i.worldDirection, UV, depth, i.time);

		//Linear blend
		float3 blendedColor = lerp(fogColor.rgb, screenColor.rgb, fogColor.a);

		//Keep alpha channel for FXAA
		return float4(blendedColor.rgb, screenColor.a);
	}


	float4 FragBlendScattering(v2f i) : SV_Target
	{
		STEREO_EYE_INDEX_POST_VERTEX(i);

		half4 screenColor = ScreenColor(UV_VR);
		half4 bloom = SAMPLE_RT(_BloomTex, Clamp, UV_VR) * _ScatteringParams.y;

		float depth = SAMPLE_DEPTH(UV_VR);
		float3 worldPos = i.worldDirection * LINEAR_EYE_DEPTH(depth) + _WorldSpaceCameraPos;
		
		//Alpha is density, do not modify
		float4 fogColor = ComputeFog(worldPos, i.worldDirection, UV, depth, i.time);

		fogColor.rgb = fogColor.rgb + bloom.rgb;

		screenColor.rgb = lerp(bloom.rgb, screenColor.rgb, fogColor.a);

		//Linear blend
		float3 blendedColor = lerp(fogColor.rgb, screenColor.rgb, fogColor.a);

		//Keep alpha channel for FXAA
		return float4(blendedColor.rgb, screenColor.a);
	}

	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass //0
		{
			Name "Fog: Light Scattering Prefilter"
			HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment FragPrefilter
			ENDHLSL
		}

		Pass //1
		{
			Name "Fog: Light Scattering Downsample"
			HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment FragDownsample
			ENDHLSL
		}
		Pass //2
		{
			Name "Fog: Light Scattering Upsample"
			HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment FragUpsample
			ENDHLSL
		}
		Pass //3
		{
			Name "Fog: Composite"
			HLSLPROGRAM
			#pragma vertex VertWorldSpaceReconstruction
			#pragma fragment FragBlend
			ENDHLSL
		}
		Pass //4
		{
			Name "Fog: Light Scattering Composite"
			HLSLPROGRAM
			#pragma vertex VertWorldSpaceReconstruction
			#pragma fragment FragBlendScattering
			ENDHLSL
		}
	}
}