using UnityEngine;
using System.Collections.Generic;

namespace PaintIn3D
{
	/// <summary>This class includes useful methods used by many other classes.</summary>
	public static partial class P3dHelper
	{
		public const string HelpUrlPrefix = "http://carloswilkes.github.io/Documentation/PaintIn3D#";

		public const string ComponentMenuPrefix = "Paint in 3D/P3D ";

		public const string ComponentHitMenuPrefix = "Paint in 3D/Hit/P3D ";

		public static System.Action<Camera> OnCameraDraw;

		private static int uniqueSeed;

		private static List<Random.State> states = new List<Random.State>();

		private static Stack<RenderTexture> actives = new Stack<RenderTexture>();

		static P3dHelper()
		{
			Camera.onPreCull += (camera) =>
				{
					if (OnCameraDraw != null) OnCameraDraw(camera);
				};

#if UNITY_2019_1_OR_NEWER
			UnityEngine.Rendering.RenderPipelineManager.beginCameraRendering += (context, camera) =>
				{
					if (OnCameraDraw != null) OnCameraDraw(camera);
				};
#elif UNITY_2018_1_OR_NEWER
			UnityEngine.Experimental.Rendering.RenderPipeline.beginCameraRendering += (camera) =>
				{
					if (OnCameraDraw != null) OnCameraDraw(camera);
				};
#endif
		}

		public static Color ToGamma(Color linear)
		{
			if (QualitySettings.activeColorSpace == ColorSpace.Linear)
			{
				return linear.gamma;
			}

			return linear;
		}

		public static Color FromGamma(Color gamma)
		{
			if (QualitySettings.activeColorSpace == ColorSpace.Linear)
			{
				return gamma.linear;
			}

			return gamma;
		}

		public static void BeginActive(RenderTexture renderTexture)
		{
			actives.Push(RenderTexture.active);

			RenderTexture.active = renderTexture;
		}

		public static void EndActive()
		{
			RenderTexture.active = actives.Pop();
		}

		public static void BeginUniqueSeed()
		{
			uniqueSeed += Random.Range(int.MinValue, int.MaxValue);

			BeginSeed(uniqueSeed);
		}

		public static void BeginSeed(int seed)
		{
			states.Add(Random.state);

			Random.InitState(seed);
		}

		public static void EndSeed()
		{
			if (states.Count > 0)
			{
				var index = states.Count - 1;

				Random.state = states[index];

				states.RemoveAt(index);
			}
		}

		public static float RatioToPercentage(float ratio01, int decimalPlaces)
		{
			var percentage = Mathf.Clamp01(ratio01) * 100.0;
			var multiplier = 1.0;

			if (decimalPlaces >= 0)
			{
				multiplier = System.Math.Pow(10.0, decimalPlaces);
			}

			return (float)(System.Math.Truncate(percentage * multiplier) / multiplier);
		}

		public static Quaternion NormalToCameraRotation(Vector3 normal, Camera optionalCamera = null)
		{
			var up     = Vector3.up;
			var camera = GetCamera(optionalCamera);

			if (camera != null)
			{
				up = camera.transform.up;
			}

			return Quaternion.LookRotation(-normal, up);
		}

		// Return the current camera, or the main camera
		public static Camera GetCamera(Camera camera = null)
		{
			if (camera == null || camera.isActiveAndEnabled == false)
			{
				camera = Camera.main;
			}

			return camera;
		}

		public static Vector3 GetCameraUp(Camera camera = null)
		{
			camera = GetCamera(camera);

			return camera != null ? camera.transform.up : Vector3.up;
		}

		public static bool IndexInMask(int index, int mask)
		{
			mask &= 1 << index;

			return mask != 0;
		}

		public static RenderTexture GetRenderTexture(RenderTexture other)
		{
			return GetRenderTexture(other.descriptor, other);
		}

		public static RenderTexture GetRenderTexture(RenderTextureDescriptor desc, RenderTexture other)
		{
			var renderTexture = GetRenderTexture(desc);

			renderTexture.filterMode = other.filterMode;
			renderTexture.anisoLevel = other.anisoLevel;
			renderTexture.wrapModeU  = other.wrapModeU;
			renderTexture.wrapModeV  = other.wrapModeV;

			return renderTexture;
		}

		public static RenderTexture GetRenderTexture(RenderTextureDescriptor desc)
		{
			return GetRenderTexture(desc, QualitySettings.activeColorSpace == ColorSpace.Gamma);
		}

		public static RenderTexture GetRenderTexture(RenderTextureDescriptor desc, bool sRGB)
		{
			desc.sRGB = sRGB;

			var renderTexture = RenderTexture.GetTemporary(desc);

			// TODO: For some reason RenderTexture.GetTemporary ignores the useMipMap flag?!
			if (renderTexture.useMipMap != desc.useMipMap)
			{
				renderTexture.Release();

				renderTexture.descriptor = desc;

				renderTexture.Create();
			}

			renderTexture.DiscardContents();

			return renderTexture;
		}

		public static RenderTexture ReleaseRenderTexture(RenderTexture renderTexture)
		{
			RenderTexture.ReleaseTemporary(renderTexture);

			return null;
		}

		public static bool CanReadPixels(TextureFormat format)
		{
			if (format == TextureFormat.RGBA32 || format == TextureFormat.ARGB32 || format == TextureFormat.RGB24 || format == TextureFormat.RGBAFloat || format == TextureFormat.RGBAHalf)
			{
				return true;
			}

			return false;
		}

		public static void ReadPixelsLinearGamma(Texture2D texture2D, RenderTexture renderTexture)
		{
			if (renderTexture != null)
			{
				BeginActive(renderTexture);

				var buffer = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false, true);

				buffer.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

				EndActive();

				var pixels = buffer.GetPixels();

				for (var i = pixels.Length - 1; i >= 0; i--)
				{
					pixels[0] = pixels[0].gamma;
				}

				Object.DestroyImmediate(buffer);

				texture2D.SetPixels(pixels);
				texture2D.Apply();
			}
		}

		public static void ReadPixels(Texture2D texture2D, RenderTexture renderTexture)
		{
			if (renderTexture != null)
			{
				BeginActive(renderTexture);

				if (CanReadPixels(texture2D.format) == true)
				{
					texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

					EndActive();

					texture2D.Apply();
				}
				else
				{
					var buffer = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);

					buffer.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

					EndActive();

					var pixels = buffer.GetPixels32();

					Object.DestroyImmediate(buffer);

					texture2D.SetPixels32(pixels);
					texture2D.Apply();
				}
			}
		}

		public static bool Downsample(RenderTexture renderTexture, int steps, ref RenderTexture temporary)
		{
			if (steps > 0 && renderTexture != null)
			{
				// Perform initial downsample to get buffer
				var oldActive         = RenderTexture.active;
				var desc              = new RenderTextureDescriptor(renderTexture.width / 2, renderTexture.height / 2, renderTexture.format, 0);
				var halfRenderTexture = GetRenderTexture(desc);

				P3dCommandReplace.BlitFast(halfRenderTexture, renderTexture, Color.white);

				// Ping-pong downsample
				for (var i = 1; i < steps; i++)
				{
					desc.width       /= 2;
					desc.height      /= 2;
					renderTexture     = halfRenderTexture;
					halfRenderTexture = GetRenderTexture(desc);

					Graphics.Blit(renderTexture, halfRenderTexture);

					ReleaseRenderTexture(renderTexture);
				}

				temporary = halfRenderTexture;

				RenderTexture.active = oldActive;

				return true;
			}

			return false;
		}

		public static bool HasMipMaps(Texture texture)
		{
			if (texture != null)
			{
				var texture2D = texture as Texture2D;

				if (texture2D != null)
				{
					return texture2D.mipmapCount > 0;
				}

				var textureRT = texture as RenderTexture;

				if (textureRT != null)
				{
					return textureRT.useMipMap;
				}
			}

			return false;
		}

		private static Mesh sphereMesh;
		private static bool sphereMeshSet;

		public static Mesh GetSphereMesh()
		{
			if (sphereMeshSet == false)
			{
				var gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

				sphereMeshSet = true;
				sphereMesh    = gameObject.GetComponent<MeshFilter>().sharedMesh;

				Object.DestroyImmediate(gameObject);
			}

			return sphereMesh;
		}

		private static Mesh quadMesh;
		private static bool quadMeshSet;

		public static Mesh GetQuadMesh()
		{
			if (quadMeshSet == false)
			{
				var gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);

				quadMeshSet = true;
				quadMesh    = gameObject.GetComponent<MeshFilter>().sharedMesh;

				Object.DestroyImmediate(gameObject);
			}

			return quadMesh;
		}

		private static Texture2D tempReadTexture;

		public static Color GetPixel(RenderTexture renderTexture, Vector2 uv, bool mipMaps = false)
		{
			if (renderTexture != null)
			{
				if (tempReadTexture == null)
				{
					tempReadTexture = new Texture2D(2, 2, TextureFormat.ARGB32, mipMaps, QualitySettings.activeColorSpace == ColorSpace.Linear);
				}

				if (SystemInfo.graphicsUVStartsAtTop == true)
				{
					uv.y = 1.0f - uv.y;
				}

				var x = uv.x * renderTexture.width;
				var y = uv.y * renderTexture.height;

				BeginActive(renderTexture);
					tempReadTexture.ReadPixels(new Rect(x, y, 1, 1), 0, 0);
				EndActive();

				tempReadTexture.Apply();

				return ToGamma(tempReadTexture.GetPixel(0, 0));
			}

			return default(Color);
		}

		public static Texture2D GetReadableCopy(Texture texture, TextureFormat format = TextureFormat.ARGB32, bool mipMaps = false, int width = 0, int height = 0)
		{
			var newTexture = default(Texture2D);

			if (texture != null)
			{
				if (width <= 0)
				{
					width = texture.width;
				}

				if (height <= 0)
				{
					height = texture.height;
				}

				if (CanReadPixels(format) == true)
				{
					var desc          = new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32, 0);
					var renderTexture = GetRenderTexture(desc, true);

					newTexture = new Texture2D(width, height, format, mipMaps, false);

					BeginActive(renderTexture);
						Graphics.Blit(texture, renderTexture);

						newTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
					EndActive();

					ReleaseRenderTexture(renderTexture);

					newTexture.Apply();
				}
			}

			return newTexture;
		}

		public static void Swap<T>(ref T a, ref T b)
		{
			var t = a;

			a = b;
			b = t;
		}

		/// <summary>This method allows you to save a byte array to PlayerPrefs, and is used by the texture saving system.
		/// If you want to save to files instead then just modify this.</summary>
		public static void SaveBytes(string saveName, byte[] data, bool save = true)
		{
			var base64 = default(string);

			if (data != null)
			{
				base64 = System.Convert.ToBase64String(data);
			}

			PlayerPrefs.SetString(saveName, base64);

			if (save == true)
			{
				PlayerPrefs.Save();
			}
		}

		/// <summary>This method allows you to load a byte array from PlayerPrefs, and is used by the texture loading system.
		/// If you want to save to files instead then just modify this.</summary>
		public static byte[] LoadBytes(string saveName)
		{
			var base64 = PlayerPrefs.GetString(saveName);

			if (string.IsNullOrEmpty(base64) == false)
			{
				return System.Convert.FromBase64String(base64);
			}

			return null;
		}

		/// <summary>This method tells if you if there exists save data at the specified save name.</summary>
		public static bool SaveExists(string saveName)
		{
			return PlayerPrefs.HasKey(saveName);
		}

		/// <summary>This method allows you to clear save data at the specified save name.</summary>
		public static void ClearSave(string saveName, bool save = true)
		{
			if (PlayerPrefs.HasKey(saveName) == true)
			{
				PlayerPrefs.DeleteKey(saveName);

				if (save == true)
				{
					PlayerPrefs.Save();
				}
			}
		}

		public static Vector3 GetPosition(Vector3 position, Vector3 endPosition)
		{
			return (position + endPosition) / 2.0f;
		}

		public static Vector3 GetPosition(Vector3 positionA, Vector3 positionB, Vector3 positionC)
		{
			return (positionA + positionB + positionC) / 3.0f;
		}

		public static Vector3 GetPosition(Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2)
		{
			return (position + position2 + endPosition + endPosition2) / 4.0f;
		}

		public static float GetRadius(Vector3 size)
		{
			return Mathf.Sqrt(Vector3.Dot(size, size));
		}

		public static float GetRadius(Vector3 size, Vector3 position, Vector3 endPosition)
		{
			size.x = System.Math.Abs(size.x) + System.Math.Abs(endPosition.x - position.x);
			size.y = System.Math.Abs(size.y) + System.Math.Abs(endPosition.y - position.y);
			size.z = System.Math.Abs(size.z) + System.Math.Abs(endPosition.z - position.z);

			return GetRadius(size);
		}

		public static float GetRadius(Vector3 size, Vector3 positionA, Vector3 positionB, Vector3 positionC)
		{
			var minX = System.Math.Min(System.Math.Min(positionA.x, positionB.x), positionC.x);
			var maxX = System.Math.Max(System.Math.Max(positionA.x, positionB.x), positionC.x);
			var minY = System.Math.Min(System.Math.Min(positionA.y, positionB.y), positionC.y);
			var maxY = System.Math.Max(System.Math.Max(positionA.y, positionB.y), positionC.y);
			var minZ = System.Math.Min(System.Math.Min(positionA.z, positionB.z), positionC.z);
			var maxZ = System.Math.Max(System.Math.Max(positionA.z, positionB.z), positionC.z);

			size.x = System.Math.Abs(size.x) + System.Math.Abs(maxX - minX);
			size.y = System.Math.Abs(size.y) + System.Math.Abs(maxY - minY);
			size.z = System.Math.Abs(size.z) + System.Math.Abs(maxZ - minZ);

			return GetRadius(size);
		}

		public static float GetRadius(Vector3 size, Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2)
		{
			var minX = System.Math.Min(System.Math.Min(position.x, position2.x), System.Math.Min(endPosition.x, endPosition2.x));
			var maxX = System.Math.Max(System.Math.Max(position.x, position2.x), System.Math.Max(endPosition.x, endPosition2.x));
			var minY = System.Math.Min(System.Math.Min(position.y, position2.y), System.Math.Min(endPosition.y, endPosition2.y));
			var maxY = System.Math.Max(System.Math.Max(position.y, position2.y), System.Math.Max(endPosition.y, endPosition2.y));
			var minZ = System.Math.Min(System.Math.Min(position.z, position2.z), System.Math.Min(endPosition.z, endPosition2.z));
			var maxZ = System.Math.Max(System.Math.Max(position.z, position2.z), System.Math.Max(endPosition.z, endPosition2.z));

			size.x = System.Math.Abs(size.x) + System.Math.Abs(maxX - minX);
			size.y = System.Math.Abs(size.y) + System.Math.Abs(maxY - minY);
			size.z = System.Math.Abs(size.z) + System.Math.Abs(maxZ - minZ);

			return GetRadius(size);
		}

		public static Vector3 ScaleAspect(Vector3 size, float aspect)
		{
			if (aspect > 1.0f)
			{
				size.y /= aspect;
			}
			else
			{
				size.x *= aspect;
			}

			return size;
		}

		public static float GetAspect(Texture textureA, Texture textureB = null)
		{
			if (textureA != null)
			{
				return textureA.width / (float)textureA.height;
			}

			if (textureB != null)
			{
				return textureB.width / (float)textureB.height;
			}

			return 1.0f;
		}

		public static void Blit(RenderTexture renderTexture, Texture other)
		{
			var oldActive = RenderTexture.active;

			Graphics.Blit(other, renderTexture);

			RenderTexture.active = oldActive;
		}

		public static void Blit(RenderTexture renderTexture, Material material, int pass)
		{
			BeginActive(renderTexture);

			P3dHelper.Draw(material, pass);
			//Graphics.Blit(default(Texture), renderTexture, material);

			EndActive();
		}

		public static Vector4 IndexToVector(int index)
		{
			switch (index)
			{
				case 0: return new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
				case 1: return new Vector4(0.0f, 1.0f, 0.0f, 0.0f);
				case 2: return new Vector4(0.0f, 0.0f, 1.0f, 0.0f);
				case 3: return new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
			}

			return default(Vector4);
		}

		public static void Draw(Material material, int pass, Mesh mesh, Matrix4x4 matrix, int subMesh, P3dCoord coord)
		{
			material.SetVector(P3dShader._Coord, IndexToVector((int)coord));

			if (material.SetPass(pass) == true)
			{
				Graphics.DrawMeshNow(mesh, matrix, subMesh);
			}
		}

		public static void Draw(Material material, int pass)
		{
			if (material.SetPass(pass) == true)
			{
				Graphics.DrawMeshNow(GetQuadMesh(), Matrix4x4.identity, 0);
			}
		}

		public static Texture2D CreateTexture(int width, int height, TextureFormat format, bool mipMaps)
		{
			if (width > 0 && height > 0)
			{
				return new Texture2D(width, height, format, mipMaps);
			}

			return null;
		}

		// This method allows you to easily find a Material attached to a GameObject
		public static Material GetMaterial(Renderer renderer, int materialIndex = 0)
		{
			if (renderer != null && materialIndex >= 0)
			{
				var materials = renderer.sharedMaterials;

				if (materialIndex < materials.Length)
				{
					return materials[materialIndex];
				}
			}

			return null;
		}

		// This method allows you to easily duplicate a Material attached to a GameObject
		public static Material CloneMaterial(GameObject gameObject, int materialIndex = 0)
		{
			if (gameObject != null && materialIndex >= 0)
			{
				var renderer = gameObject.GetComponent<Renderer>();

				if (renderer != null)
				{
					var materials = renderer.sharedMaterials;

					if (materialIndex < materials.Length)
					{
						// Get existing material
						var material = materials[materialIndex];

						// Clone it
						material = Object.Instantiate(material);

						// Update array
						materials[materialIndex] = material;

						// Update materials
						renderer.sharedMaterials = materials;

						return material;
					}
				}
			}

			return null;
		}

		// This method allows you to add a material (layer) to a renderer at the specified material index, or -1 for the end (top)
		public static Material AddMaterial(Renderer renderer, Shader shader, int materialIndex = -1)
		{
			if (renderer != null)
			{
				var newMaterials = new List<Material>(renderer.sharedMaterials);
				var newMaterial  = new Material(shader);

				if (materialIndex <= 0)
				{
					materialIndex = newMaterials.Count;
				}

				newMaterials.Insert(materialIndex, newMaterial);

				renderer.sharedMaterials = newMaterials.ToArray();

				return newMaterial;
			}

			return null;
		}

		public static float Reciprocal(float a)
		{
			return a != 0.0f ? 1.0f / a : 0.0f;
		}

		public static float Divide(float a, float b)
		{
			return b != 0.0f ? a / b : 0.0f;
		}

		public static int Mod(int a, int b)
		{
			var m = a % b;

			if (m < 0)
			{
				return m + b;
			}

			return m;
		}

		public static Vector3 Reciprocal3(Vector3 xyz)
		{
			xyz.x = Reciprocal(xyz.x);
			xyz.y = Reciprocal(xyz.y);
			xyz.z = Reciprocal(xyz.z);

			return xyz;
		}

		public static float DampenFactor(float dampening, float elapsed)
		{
			if (dampening < 0.0f)
			{
				return 1.0f;
			}
#if UNITY_EDITOR
			if (Application.isPlaying == false)
			{
				return 1.0f;
			}
#endif
			return 1.0f - Mathf.Pow((float)System.Math.E, -dampening * elapsed);
		}

		// This allows you to destroy a UnityEngine.Object in edit or play mode
		public static T Destroy<T>(T o)
			where T : Object
		{
#if UNITY_EDITOR
			if (Application.isPlaying == false)
			{
				Object.DestroyImmediate(o, true); return null;
			}
#endif

			Object.Destroy(o);

			return null;
		}
	}
}