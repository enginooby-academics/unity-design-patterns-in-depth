/*
Copyright(C) 2015 Keijiro Takahashi

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files(the "Software"),
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
and / or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions :

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

Shader "Hidden/SC Post Effects/Kaleidoscope"
{
	HLSLINCLUDE

	#include "../../../Shaders/Pipeline/Pipeline.hlsl"

	float _Splits;

	float4 Frag(Varyings i) : SV_Target
	{
		STEREO_EYE_INDEX_POST_VERTEX(i);

		// Convert to the polar coordinate.
		float2 sc = UV_VR- 0.5;
		float phi = atan2(sc.y, sc.x);
		float r = sqrt(dot(sc, sc));

		// Angular repeating.
		phi = phi - _Splits * floor(phi / _Splits);
		phi = min(phi, _Splits - phi);

		// Convert back to the texture coordinate.
		float2 uv = float2(cos(phi), sin(phi)) * r + 0.5;

		// Reflection at the border of the screen.
		uv = max(min(uv, 2.0 - uv), -uv);

		return ScreenColor(uv);
	}

	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			Name "Kaleidoscope"
			HLSLPROGRAM

			#pragma vertex Vert
			#pragma fragment Frag

			ENDHLSL
		}
	}
}