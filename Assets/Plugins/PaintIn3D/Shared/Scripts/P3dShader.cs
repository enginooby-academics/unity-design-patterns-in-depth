using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This class caches shader information, speeding them up.</summary>
	public static class P3dShader
	{
		public static int _Buffer;
		public static int _BufferSize;
		public static int _CameraOffset;
		public static int _Channels;
		public static int _ChannelR;
		public static int _ChannelG;
		public static int _ChannelB;
		public static int _ChannelA;
		public static int _Color;
		public static int _Coord;
		public static int _Direction;
		public static int _EndPosition;
		public static int _EndPosition2;
		public static int _Hardness;
		public static int _In3D;
		public static int _Kernel;
		public static int _Position;
		public static int _Position2;
		public static int _ReplaceTexture;
		public static int _ReplaceTextureSize;
		public static int _ReplaceColor;
		public static int _LocalMaskTexture;
		public static int _LocalMaskChannel;
		public static int _MaskMatrix;
		public static int _MaskTexture;
		public static int _MaskChannel;
		public static int _MaskStretch;
		public static int _Matrix;
		public static int _Minimum;
		public static int _NormalBack;
		public static int _NormalFront;
		public static int _Opacity;
		public static int _Shape;
		public static int _ShapeChannel;
		public static int _Texture;
		public static int _TextureR;
		public static int _TextureG;
		public static int _TextureB;
		public static int _TextureA;
		public static int _TileTexture;
		public static int _TileMatrix;
		public static int _TileOpacity;
		public static int _TileTransition;
		public static int _Wrapping;

		static P3dShader()
		{
			_Buffer = Shader.PropertyToID("_Buffer");
			_BufferSize = Shader.PropertyToID("_BufferSize");
			_CameraOffset = Shader.PropertyToID("_CameraOffset");
			_Channels = Shader.PropertyToID("_Channels");
			_ChannelR = Shader.PropertyToID("_ChannelR");
			_ChannelG = Shader.PropertyToID("_ChannelG");
			_ChannelB = Shader.PropertyToID("_ChannelB");
			_ChannelA = Shader.PropertyToID("_ChannelA");
			_Color = Shader.PropertyToID("_Color");
			_Coord = Shader.PropertyToID("_Coord");
			_Direction = Shader.PropertyToID("_Direction");
			_EndPosition = Shader.PropertyToID("_EndPosition");
			_EndPosition2 = Shader.PropertyToID("_EndPosition2");
			_Hardness = Shader.PropertyToID("_Hardness");
			_In3D = Shader.PropertyToID("_In3D");
			_Kernel = Shader.PropertyToID("_Kernel");
			_Position = Shader.PropertyToID("_Position");
			_Position2 = Shader.PropertyToID("_Position2");
			_ReplaceTexture = Shader.PropertyToID("_ReplaceTexture");
			_ReplaceTextureSize = Shader.PropertyToID("_ReplaceTextureSize");
			_ReplaceColor = Shader.PropertyToID("_ReplaceColor");
			_LocalMaskTexture = Shader.PropertyToID("_LocalMaskTexture");
			_LocalMaskChannel = Shader.PropertyToID("_LocalMaskChannel");
			_MaskMatrix = Shader.PropertyToID("_MaskMatrix");
			_MaskTexture = Shader.PropertyToID("_MaskTexture");
			_MaskChannel = Shader.PropertyToID("_MaskChannel");
			_MaskStretch = Shader.PropertyToID("_MaskStretch");
			_Matrix = Shader.PropertyToID("_Matrix");
			_Minimum = Shader.PropertyToID("_Minimum");
			_NormalBack = Shader.PropertyToID("_NormalBack");
			_NormalFront = Shader.PropertyToID("_NormalFront");
			_Opacity = Shader.PropertyToID("_Opacity");
			_Shape = Shader.PropertyToID("_Shape");
			_ShapeChannel = Shader.PropertyToID("_ShapeChannel");
			_Texture = Shader.PropertyToID("_Texture");
			_TextureR = Shader.PropertyToID("_TextureR");
			_TextureG = Shader.PropertyToID("_TextureG");
			_TextureB = Shader.PropertyToID("_TextureB");
			_TextureA = Shader.PropertyToID("_TextureA");
			_TileTexture = Shader.PropertyToID("_TileTexture");
			_TileMatrix = Shader.PropertyToID("_TileMatrix");
			_TileOpacity = Shader.PropertyToID("_TileOpacity");
			_TileTransition = Shader.PropertyToID("_TileTransition");
			_Wrapping = Shader.PropertyToID("_Wrapping");
		}

		public static Shader Load(string shaderName)
		{
			var shader = Shader.Find(shaderName);

			if (shader == null)
			{
				throw new System.Exception("Failed to find shader called: " + shaderName);
			}

			return shader;
		}

		public static Material Build(Shader shader)
		{
			var material = new Material(shader);
#if UNITY_EDITOR
			material.hideFlags = HideFlags.DontSave;
#endif
			return material;
		}

		public static Material BuildMaterial(string shaderName, string keyword = null)
		{
			var shader   = Load(shaderName);
			var material = Build(shader);

			material.name = shaderName + keyword;

			if (string.IsNullOrEmpty(keyword) == false)
			{
				material.EnableKeyword(keyword);
			}

			return material;
		}
	}
}