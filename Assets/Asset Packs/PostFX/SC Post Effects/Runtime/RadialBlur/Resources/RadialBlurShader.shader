Shader "Hidden/SC Post Effects/Radial Blur"
{
	HLSLINCLUDE

	#include "../../../Shaders/Pipeline/Pipeline.hlsl"

	float _Amount;
	float _Iterations;

	float4 Frag(Varyings i) : SV_Target
	{
		STEREO_EYE_INDEX_POST_VERTEX(i);

		float2 blurVector = (float2(0.5, 0.5) - UV.xy) * _Amount;

		half4 color = half4(0,0,0,0);
		[unroll(12)]
		for (int j = 0; j < _Iterations; j++)
		{
			half4 screenColor = ScreenColor(UV_VR);
			color += screenColor;
			UV_VR.xy += blurVector;
		}

		return color / _Iterations;
	}

	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			Name "Radial Blur"
			HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag

			ENDHLSL
		}
	}
}