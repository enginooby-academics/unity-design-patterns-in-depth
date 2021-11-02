Shader "Hidden/SC Post Effects/Sharpen"
{
	HLSLINCLUDE

	#include "../../../Shaders/Pipeline/Pipeline.hlsl"

	float4 _Params;
	//X: Amount
	//Y: Radius

	float4 Frag(Varyings i) : SV_Target
	{
		STEREO_EYE_INDEX_POST_VERTEX(i);

		float2 uv = UV_VR;
		float4 screenColor = ScreenColor(UV_VR);

		float2 sampleDistance = 1.0 / _ScreenParams.xy;
		float3 sampleTL = ScreenColor(uv + float2(-sampleDistance.x, -sampleDistance.y) * _Params.y).rgb;
		float3 sampleTR = ScreenColor(uv + float2(sampleDistance.x, -sampleDistance.y) * _Params.y).rgb;
		float3 sampleBL = ScreenColor(uv + float2(-sampleDistance.x, sampleDistance.y) * _Params.y).rgb;
		float3 sampleBR = ScreenColor(uv + float2(sampleDistance.x, sampleDistance.y) * _Params.y).rgb;

		float3 offsetColors = 0.25 * (sampleTL + sampleTR + sampleBL + sampleBR);

		float3 sourceColor = screenColor.rgb;
		#ifdef URP //Effect can only execute before Bloom, treat as LDR
		sourceColor = saturate(sourceColor);
		#endif
		float3 sharpenedColor = screenColor.rgb + (sourceColor.rgb - offsetColors) * _Params.x;

		return float4(sharpenedColor.rgb, screenColor.a);
	}

	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			Name "Sharpen"
			HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag

			ENDHLSL
		}
	}
}