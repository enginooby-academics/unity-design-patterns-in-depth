using System.Collections.Generic;
using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This class manages the fill painting command.</summary>
	public class P3dCommandFill : P3dCommand
	{
		public P3dBlendMode     Blend;
		public P3dHashedTexture Texture;
		public Color            Color;
		public float            Opacity;
		public float            Minimum;

		public static P3dCommandFill Instance = new P3dCommandFill();

		private static Stack<P3dCommandFill> pool = new Stack<P3dCommandFill>();

		private static Material cachedMaterial;

		private static int cachedMaterialHash;

		public override bool RequireMesh { get { return false; } }

		static P3dCommandFill()
		{
			BuildMaterial(ref cachedMaterial, ref cachedMaterialHash, "Hidden/Paint in 3D/Fill");
		}

		public static RenderTexture Blit(RenderTexture main, P3dBlendMode blendMode, Texture texture, Color color, float opacity, float minimum)
		{
			var swap = P3dHelper.GetRenderTexture(main.descriptor, main);

			Blit(ref main, ref swap, blendMode, texture, color, opacity, minimum);

			P3dHelper.ReleaseRenderTexture(swap);

			return main;
		}

		public static void Blit(ref RenderTexture main, ref RenderTexture swap, P3dBlendMode blendMode, Texture texture, Color color, float opacity, float minimum)
		{
			var material = Instance.SetMaterial(blendMode, texture, color, opacity, minimum);

			Instance.Apply(material);

			//if (doubleBuffer == true)
			{
				P3dCommandReplace.Blit(swap, main, Color.white);

				material.SetTexture(P3dShader._Buffer, swap);
				material.SetVector(P3dShader._BufferSize, new Vector2(swap.width, swap.height));
			}

			P3dHelper.Blit(main, material, blendMode);
		}

		public override void Apply(Material material)
		{
			base.Apply(material);

			Blend.Apply(material);

			material.SetTexture(P3dShader._Texture, Texture);
			material.SetColor(P3dShader._Color, P3dHelper.FromGamma(Color));
			material.SetFloat(P3dShader._Opacity, Opacity);
			material.SetVector(P3dShader._Minimum, new Vector4(Minimum, Minimum, Minimum, Minimum));
		}

		public override void Pool()
		{
			pool.Push(this);
		}

		public override void Transform(Matrix4x4 posMatrix, Matrix4x4 rotMatrix)
		{
		}

		public override P3dCommand SpawnCopy()
		{
			var command = SpawnCopy(pool);

			command.Blend   = Blend;
			command.Texture = Texture;
			command.Color   = Color;
			command.Opacity = Opacity;
			command.Minimum = Minimum;

			return command;
		}

		public override void Apply(P3dPaintableTexture paintableTexture)
		{
			base.Apply(paintableTexture);

			if (Blend.Index == P3dBlendMode.REPLACE_ORIGINAL)
			{
				Blend.Color   = paintableTexture.Color;
				Blend.Texture = paintableTexture.Texture;
			}
		}

		public Material SetMaterial(P3dBlendMode blendMode, Texture texture, Color color, float opacity, float minimum)
		{
			Blend    = blendMode;
			Material = new P3dHashedMaterial(cachedMaterial, cachedMaterialHash);
			Pass     = blendMode;
			Texture  = texture;
			Color    = color;
			Opacity  = opacity;
			Minimum  = minimum;

			return cachedMaterial;
		}
	}
}