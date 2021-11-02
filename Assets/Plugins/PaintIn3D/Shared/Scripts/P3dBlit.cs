using UnityEngine;

namespace PaintIn3D
{
	public static class P3dBlit
	{
		private static Material cachedWhite;

		private static bool cachedWhiteSet;

		private static Material cachedTexture;

		private static bool cachedTextureSet;

		private static Material cachedNormal;

		private static bool cachedNormalSet;

		private static Material cachedPremultiply;

		private static bool cachedPremultiplySet;

		public static void White(RenderTexture rendertexture, Mesh mesh, int submesh, P3dCoord coord)
		{
			P3dHelper.BeginActive(rendertexture);

			if (mesh != null)
			{
				if (cachedWhiteSet == false)
				{
					cachedWhite    = P3dShader.BuildMaterial("Hidden/Paint in 3D/White");
					cachedWhiteSet = true;
				}

				GL.Clear(true, true, Color.black);

				P3dHelper.Draw(cachedWhite, 0, mesh, Matrix4x4.identity, submesh, coord);
			}
			else
			{
				GL.Clear(true, true, Color.white);
			}

			P3dHelper.EndActive();
		}

		public static void Texture(RenderTexture rendertexture, Mesh mesh, int submesh, Texture texture, P3dCoord coord)
		{
			if (cachedTextureSet == false)
			{
				cachedTexture    = P3dShader.BuildMaterial("Hidden/Paint in 3D/Texture");
				cachedTextureSet = true;
			}

			P3dHelper.BeginActive(rendertexture);

			cachedTexture.SetTexture(P3dShader._Buffer, texture);
			cachedTexture.SetVector(P3dShader._BufferSize, new Vector2(texture.width, texture.height));

			P3dHelper.Draw(cachedTexture, 0, mesh, Matrix4x4.identity, submesh, coord);

			P3dHelper.EndActive();
		}

		public static void Normal(RenderTexture rendertexture, Texture texture)
		{
			if (cachedNormalSet == false)
			{
				cachedNormal    = P3dShader.BuildMaterial("Hidden/Paint in 3D/Normal");
				cachedNormalSet = true;
			}

			cachedNormal.SetTexture(P3dShader._Texture, texture);

			P3dHelper.Blit(rendertexture, cachedNormal, 0);
		}

		public static void Premultiply(RenderTexture rendertexture, Texture texture, Color tint)
		{
			if (cachedPremultiplySet == false)
			{
				cachedPremultiply    = P3dShader.BuildMaterial("Hidden/Paint in 3D/Premultiply");
				cachedPremultiplySet = true;
			}

			cachedPremultiply.SetTexture(P3dShader._Texture, texture);
			cachedPremultiply.SetColor(P3dShader._Color, P3dHelper.FromGamma(tint));

			P3dHelper.Blit(rendertexture, cachedPremultiply, 0);
		}
	}
}