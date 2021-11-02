float3 _Position;
float3 _EndPosition;
float3 _Position2;
float3 _EndPosition2;

float3 GetClosestPosition_Edge(float3 a, float3 b, float3 p)
{
	float3 ab = b - a;

	return a + ab * saturate(dot(p - a, ab) / dot(ab, ab));
}

float3 GetClosestPosition_Triangle(float3 a, float3 b, float3 c, float3 p)
{
	float3 ab = b - a;
	float3 bc = c - b;
	float3 ca = a - c;
	float3 ap = p - a;
	float3 bp = p - b;
	float3 cp = p - c;
	float3 n = cross(ab, bc);

	if (dot(cross(n, ab), ap) < 0.0f)
	{
		return a + ab * saturate(dot(ap, ab) / dot(ab, ab));
	}

	if (dot(cross(n, bc), bp) < 0.0f)
	{
		return b + bc * saturate(dot(bp, bc) / dot(bc, bc));
	}

	if (dot(cross(n, ca), cp) < 0.0f)
	{
		return c + ca * saturate(dot(cp, ca) / dot(ca, ca));
	}

	return p - n * dot(n, ap) / dot(n, n);
}

float3 GetClosestPosition(float3 p)
{
#if P3D_LINE
	return GetClosestPosition_Edge(_Position, _EndPosition, p);
#elif P3D_QUAD
	float3 a = GetClosestPosition_Triangle(_Position, _EndPosition, _Position2, p);
	float3 b = GetClosestPosition_Triangle(_EndPosition2, _Position2, _EndPosition, p);

	return length(a - p) < length(b - p) ? a : b;
#endif
	return _Position;
}