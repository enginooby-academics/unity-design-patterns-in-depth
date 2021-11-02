Shader "Hidden/SC Post Effects/Cloud Shadows"
{
	HLSLINCLUDE

	#define REQUIRE_DEPTH
	#include "../../../Shaders/Pipeline/Pipeline.hlsl"

	DECLARE_TEX(_NoiseTex);

	//Prefer high precision depth
	//#pragma fragmentoption ARB_precision_hint_nicest

	float4 _CloudParams;
	float4 _FadeParams;
	float _ProjectionEnabled;
	
	uniform float4x4 clipToWorld;
	float4x4 unity_WorldToLight;
	
	struct v2f {
		float4 positionCS : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 texcoordStereo : TEXCOORD1;
		float3 worldDirection : TEXCOORD2;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	v2f VertWSRecontruction(Attributes v) {
		v2f o;

		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		o.positionCS = OBJECT_TO_CLIP(v);
		o.uv.xy = GET_TRIANGLE_UV(v);

		o.uv = FlipUV(o.uv);

		float4 clip = float4(o.uv.xy * 2 - 1, 0.0, 1.0);
		o.worldDirection.rgb = (mul((float4x4)clipToWorld, clip.rgba).xyz - _WorldSpaceCameraPos.rgb).xyz;

		//UNITY_SINGLE_PASS_STEREO
		o.texcoordStereo = GET_TRIANGLE_UV_VR(o.uv, v.vertexID);

		return o;
	}

	float4 Frag(v2f i) : SV_Target
	{
		STEREO_EYE_INDEX_POST_VERTEX(i);

		half4 sceneColor = SCREEN_COLOR(UV_VR);
		float depth = SAMPLE_DEPTH(UV_VR);
		float3 worldPos = i.worldDirection.xyz * LINEAR_EYE_DEPTH(depth) + _WorldSpaceCameraPos.xyz;

		float2 projection = worldPos.xz;
		if(_ProjectionEnabled == 1) projection = mul((float4x4)unity_WorldToLight, float4(worldPos, 1.0)).xy * LightProjectionMultiplier;
		
		float2 uv = projection * _CloudParams.x + (_Time.y * float2(_CloudParams.y, _CloudParams.z));
		float clouds = 1- SAMPLE_TEX(_NoiseTex, sampler_LinearRepeat, uv).r;

		//Clip skybox
		if (LINEAR_DEPTH(depth) > 0.99) clouds = 1;

		float fadeFactor = LinearDistanceFade(worldPos, _FadeParams.x, _FadeParams.y, 1.0, 1.0);
		
		clouds = lerp(1, clouds, _CloudParams.w * fadeFactor);

		float3 cloudsBlend = sceneColor.rgb * clouds;

		return float4(cloudsBlend.rgb, sceneColor.a);
	}


	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			Name "Cloud Shadows"
			HLSLPROGRAM

			#pragma vertex VertWSRecontruction
			#pragma fragment Frag

			ENDHLSL
		}
	}
}