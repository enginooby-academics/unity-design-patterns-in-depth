#if UNITY_2018_2_OR_NEWER
	#define ASYNC_READBACK_SUPPORTED
#endif
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;

namespace PaintIn3D
{
	/// <summary>This class allows you to read the contents of a <b>RenderTexture</b> immediately or async.</summary>
	[System.Serializable]
	public class P3dReader
	{
#if ASYNC_READBACK_SUPPORTED
		[SerializeField]
		private AsyncGPUReadbackRequest request;
#endif
		[SerializeField]
		private bool requested;

		[SerializeField]
		private RenderTexture buffer;

		[SerializeField]
		private Vector2Int originalSize;

		[SerializeField]
		private Vector2Int downsampledSize;

		[SerializeField]
		private int downsampleSteps;

		[SerializeField]
		private int downsampleBoost;

		[SerializeField]
		private Texture2D tempTexture;

		public event System.Action<NativeArray<Color32>> OnComplete;

		public bool Requested
		{
			get
			{
				return requested;
			}
		}

		public Vector2Int OriginalSize
		{
			get
			{
				return originalSize;
			}
		}

		public int DownsampleSteps
		{
			get
			{
				return downsampleSteps;
			}
		}

		public Vector2Int DownsampledSize
		{
			get
			{
				return downsampledSize;
			}
		}

		public int DownsampleBoost
		{
			get
			{
				return downsampleBoost;
			}
		}

		public void UpdateRequest()
		{
#if ASYNC_READBACK_SUPPORTED
			if (requested == true)
			{
				if (request.hasError == true)
				{
					requested = false;

					CompleteDirectly();
				}
				else if (request.done == true)
				{
					requested = false;

					buffer = P3dHelper.ReleaseRenderTexture(buffer);

					OnComplete(request.GetData<Color32>());
				}
			}
#endif
		}

		public static bool NeedsUpdating<T>(P3dReader reader, NativeArray<T> array, RenderTexture texture, int downsampleSteps)
			where T : struct
		{
			if (array.IsCreated == false || reader.DownsampledSize.x * reader.DownsampledSize.y != array.Length)
			{
				return true;
			}

			var originalSize    = Vector2Int.zero;
			var downsampledSize = Vector2Int.zero;

			originalSize.x = downsampledSize.x = texture.width;
			originalSize.y = downsampledSize.y = texture.height;

			for (var i = 0; i < downsampleSteps; i++)
			{
				if (downsampledSize.x > 2)
				{
					downsampledSize.x /= 2;
				}

				if (downsampledSize.y > 2)
				{
					downsampledSize.y /= 2;
				}
			}

			return reader.OriginalSize != originalSize || reader.DownsampledSize != downsampledSize;
		}

		public void Request(RenderTexture texture, int downsample, bool async)
		{
			if (texture == null)
			{
				Debug.LogError("Texture null."); return;
			}

			if (requested == true)
			{
				Debug.LogError("Already requested."); return;
			}

			if (buffer != null)
			{
				Debug.LogError("Buffer exists."); return;
			}

			originalSize.x = downsampledSize.x = texture.width;
			originalSize.y = downsampledSize.y = texture.height;

			for (var i = 0; i < downsample; i++)
			{
				if (downsampledSize.x > 2)
				{
					downsampledSize.x /= 2;
				}

				if (downsampledSize.y > 2)
				{
					downsampledSize.y /= 2;
				}
			}

			downsampleSteps = downsample;
			downsampleBoost = (originalSize.x / downsampledSize.x) * (originalSize.y / downsampledSize.y);

			var desc = texture.descriptor;

			desc.useMipMap = false;
			desc.width     = downsampledSize.x;
			desc.height    = downsampledSize.y;

			buffer = P3dHelper.GetRenderTexture(desc);

			P3dCommandReplace.Blit(buffer, texture, Color.white);
#if ASYNC_READBACK_SUPPORTED
			if (async == true && SystemInfo.supportsAsyncGPUReadback == true)
			{
				request   = AsyncGPUReadback.Request(buffer, 0, TextureFormat.RGBA32);
				requested = true;

				UpdateRequest();
			}
			else
#endif
			{
				CompleteDirectly();
			}
		}

		public void Release()
		{
			buffer = P3dHelper.ReleaseRenderTexture(buffer);

			tempTexture = P3dHelper.Destroy(tempTexture);
		}

		private void CompleteDirectly()
		{
			if (tempTexture != null && (tempTexture.width != buffer.width || tempTexture.height != buffer.height))
			{
				tempTexture = P3dHelper.Destroy(tempTexture);
			}

			if (tempTexture == null)
			{
				tempTexture = new Texture2D(buffer.width, buffer.height, TextureFormat.RGBA32, false);
			}

			P3dHelper.BeginActive(buffer);

			tempTexture.ReadPixels(new Rect(0, 0, buffer.width, buffer.height), 0, 0);

			P3dHelper.EndActive();

			buffer = P3dHelper.ReleaseRenderTexture(buffer);

			tempTexture.Apply();

			OnComplete(tempTexture.GetRawTextureData<Color32>());
		}
	}
}