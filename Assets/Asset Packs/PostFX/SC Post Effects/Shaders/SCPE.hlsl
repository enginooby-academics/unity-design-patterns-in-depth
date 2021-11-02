// SC Post Effects
// Staggart Creations
// http://staggart.xyz

//-------------
// Generic functions
//-------------

//ZW components of depth-normals texture are passed in
inline float DecodeFloatRG( float2 enc )
{
	#if _RECONSTRUCT_NORMAL
	float2 kDecodeDot = float2(1.0, 1/255.0);
	return dot( enc, kDecodeDot );
	#else
	return enc.y; //W-component contains linear depth
	#endif
}

#ifdef URP
// Decode normals stored in _CameraDepthNormalsTexture
float3 DecodeViewNormalStereo(float4 enc4)
{
	float kScale = 1.7777;
	float3 nn = enc4.xyz * float3(2.0 * kScale, 2.0 * kScale, 0) + float3(-kScale, -kScale, 1);
	float g = 2.0 / dot(nn.xyz, nn.xyz);
	float3 n;
	n.xy = g * nn.xy;
	n.z = g - 1.0;
	return n;
}
#endif

inline void DecodeDepthNormal( float4 enc, out float depth, out float3 normal )
{
	depth = DecodeFloatRG (enc.zw);
	normal = DecodeViewNormalStereo (enc);
}

float rand(float n) { return frac(sin(n) * 13758.5453123 * 0.01); }

float rand(float2 n) { return frac(sin(dot(n, float2(12.9898, 4.1414))) * 43758.5453); }

float2 RotateUV(float2 uv, float rotation) {
	float cosine = cos(rotation);
	float sine = sin(rotation);
	float2 pivot = float2(0.5, 0.5);
	float2 rotator = (mul(uv - pivot, float2x2(cosine, -sine, sine, cosine)) + pivot);
	return saturate(rotator);
}

float3 ChromaticAberration(RT_ARG(tex, samplerTex), float4 texelSize, float2 uv, float amount) {
	float2 direction = normalize((float2(0.5, 0.5) - uv));
	float3 distortion = float3(-texelSize.x * amount, 0, texelSize.x * amount);

	float red = SAMPLE_RT(tex, samplerTex, uv + direction * distortion.r).r;
	float green = SAMPLE_RT(tex, samplerTex, uv + direction * distortion.g).g;
	float blue = SAMPLE_RT(tex, samplerTex, uv + direction * distortion.b).b;

	return float3(red, green, blue);
}

half4 LuminanceThreshold(half4 color, half threshold)
{
	half br = Max3(color.r, color.g, color.b);

	half contrib = max(0, br - threshold);

	contrib /= max(br, 0.001);

	return color * contrib;
}

#ifndef URP //Already defined
float Luminance(float3 color)
{
	return (color.r * 0.3 + color.g * 0.59 + color.b * 0.11);
}
#endif

// (returns 1.0 when orthographic)
float CheckPerspective(float x)
{
	return lerp(x, 1.0, unity_OrthoParams.w);
}

#define NEAR_PLANE _ProjectionParams.y
#define FAR_PLANE _ProjectionParams.z

float LinearDepthFade(float linearDepth, float start, float end, float invert, float enable)
{
	if(enable == 0.0) return 1.0;
	
	float rawDepth = (linearDepth * FAR_PLANE) - NEAR_PLANE;
	float eyeDepth = FAR_PLANE - ((_ZBufferParams.z * (1.0 - rawDepth) + _ZBufferParams.w) * _ProjectionParams.w);

	float perspDist = rawDepth;
	float orthoDist = eyeDepth;

	//Non-linear depth values
	float dist = lerp(perspDist, orthoDist, unity_OrthoParams.w);
	
	float fadeFactor = saturate((end - dist) / (end-start));

	//OpenGL
	#if !defined(UNITY_REVERSED_Z)
	fadeFactor = 1-fadeFactor;
	#endif
	
	if (invert == 1.0) fadeFactor = 1-fadeFactor;

	return fadeFactor;
}

//Having a world position, fading can be radial
float LinearDistanceFade(float3 worldPos, float start, float end, float invert, float enable)
{
	if(enable == 0.0) return 1.0;
	
	float pixelDist = length(_WorldSpaceCameraPos.xyz - worldPos.xyz);

	//Distance based scalar
	float fadeFactor = saturate((end - pixelDist ) / (end-start));

	if (invert == 1.0) fadeFactor = 1-fadeFactor;

	return fadeFactor;
}

// Reconstruct view-space position from UV and depth.
float3 ReconstructViewPos(float2 uv, float depth)
{
	float3 worldPos = float3(0, 0, 0);
	worldPos.xy = (uv.xy * 2.0 - 1.0 - float2(unity_CameraProjection._13, unity_CameraProjection._23)) / float2(unity_CameraProjection._11, unity_CameraProjection._22) * CheckPerspective(depth);
	worldPos.z = depth;
	return worldPos;
}

float2 FisheyeUV(half2 uv, half amount, half zoom)
{
	half2 center = uv.xy - half2(0.5, 0.5);
	half CdotC = dot(center, center);
	half f = 1.0 + CdotC * (amount * sqrt(CdotC));
	return f * zoom * center + 0.5;
}

float2 Distort(float2 uv)
{
#if DISTORT
	{
		uv = (uv - 0.5) * _Distortion_Amount.z + 0.5;
		float2 ruv = _Distortion_CenterScale.zw * (uv - 0.5 - _Distortion_CenterScale.xy);
		float ru = length(float2(ruv));

		UNITY_BRANCH
			if (_Distortion_Amount.w > 0.0)
			{
				float wu = ru * _Distortion_Amount.x;
				ru = tan(wu) * (1.0 / (ru * _Distortion_Amount.y));
				uv = uv + ruv * (ru - 1.0);
			}
			else
			{
				ru = (1.0 / ru) * _Distortion_Amount.x * atan(ru * _Distortion_Amount.y);
				uv = uv + ruv * (ru - 1.0);
			}
	}
#endif

	return uv;
}