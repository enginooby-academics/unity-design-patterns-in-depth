#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace PaintIn3D
{
	/// <summary>This class includes useful editor-specific methods used by many other classes.</summary>
	public static partial class P3dHelper
	{
		

		public static string FindScriptGUID<T>()
			where T : MonoBehaviour
		{
			var guids = AssetDatabase.FindAssets("t:Script " + typeof(T).Name);

			foreach (var guid in guids)
			{
				if (System.IO.Path.GetFileName(AssetDatabase.GUIDToAssetPath(guid)) == typeof(T).Name + ".cs")
				{
					return guid;
				}
			}

			return null;
		}

		public static T LoadPrefabIfItContainsScriptGUID<T>(string prefabGuid, string scriptGuid)
			where T : MonoBehaviour
		{
			var prefabPath = AssetDatabase.GUIDToAssetPath(prefabGuid);

			foreach (var line in System.IO.File.ReadLines(prefabPath))
			{
				if (line.Contains(scriptGuid) == true)
				{
					var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

					if (prefab != null)
					{
						var component = prefab.GetComponent<T>();

						if (component != null)
						{
							return component;
						}
					}

					break;
				}
			}

			return null;
		}

		public static void ClearControl()
		{
			GUIUtility.keyboardControl = -1;
			GUIUtility.hotControl      = -1;
		}

		public static void SelectAndPing(Object obj)
		{
			if (obj != null)
			{
				Selection.activeObject = obj;

				EditorGUIUtility.PingObject(obj);
			}
		}
	
		public static void SetDirty(Object target)
		{
			if (Application.isEditor == false)
			{
				UnityEditor.EditorUtility.SetDirty(target);

#if UNITY_4 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				UnityEditor.EditorApplication.MarkSceneDirty();
#else
				UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
#endif
			}
		}
	
		public static T GetAssetImporter<T>(Object asset)
			where T : AssetImporter
		{
			return GetAssetImporter<T>((AssetDatabase.GetAssetPath(asset)));
		}
	
		public static T GetAssetImporter<T>(string path)
			where T : AssetImporter
		{
			return AssetImporter.GetAtPath(path) as T;
		}
	
		public static string SaveDialog(string title, string directory, string defaultName, string extension)
		{
			var path = EditorUtility.SaveFilePanel(title, directory, defaultName, extension);
		
			if (path.StartsWith(Application.dataPath) == true)
			{
				path = "Assets" + path.Substring(Application.dataPath.Length);
			}
		
			return path;
		}
	
		public static void ReimportAsset(Object asset)
		{
			ReimportAsset(AssetDatabase.GetAssetPath(asset));
		}
	
		public static void ReimportAsset(string path)
		{
			AssetDatabase.ImportAsset(path);
		}
	
		public static bool IsAsset(Object o)
		{
			return o != null && string.IsNullOrEmpty(UnityEditor.AssetDatabase.GetAssetPath(o)) == false;
		}

		public static bool TexEnvNameExists(Shader shader, string name)
		{
			if (shader != null)
			{
				var count = ShaderUtil.GetPropertyCount(shader);
				
				for (var i = 0; i < count; i++)
				{
					if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
					{
						if (ShaderUtil.GetPropertyName(shader, i) == name)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		public struct TexEnv
		{
			public string Name;
			public string Desc;

			public string Title
			{
				get
				{
					return Desc + " (" + Name + ")";
				}
			}
		}

		private static List<TexEnv> texEnvNames = new List<TexEnv>();

		public static List<TexEnv> GetTexEnvs(Shader shader)
		{
			texEnvNames.Clear();

			if (shader != null)
			{
				var count = ShaderUtil.GetPropertyCount(shader);

				for (var i = 0; i < count; i++)
				{
					if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
					{
						var texEnv = default(TexEnv);

						texEnv.Name = ShaderUtil.GetPropertyName(shader, i);
						texEnv.Desc = ShaderUtil.GetPropertyDescription(shader, i);

						texEnvNames.Add(texEnv);
					}
				}
			}

			return texEnvNames;
		}

		public static Texture[] CopyTextures(Material material)
		{
			if (material != null)
			{
				var texEnvs  = GetTexEnvs(material.shader);
				var textures = new Texture[texEnvNames.Count];

				for (var i = texEnvNames.Count - 1; i >= 0; i--)
				{
					textures[i] = material.GetTexture(texEnvs[i].Name);
				}

				return textures;
			}

			return null;
		}

		public static void PasteTextures(Material material, Texture[] textures)
		{
			if (material != null)
			{
				var texEnvs = GetTexEnvs(material.shader);

				for (var i = texEnvNames.Count - 1; i >= 0; i--)
				{
					material.SetTexture(texEnvs[i].Name, textures[i]);
				}
			}
		}

		public static void SaveTextureAsset(Texture texture, string path, bool overwrite = false)
		{
			var texture2D = GetReadableCopy(texture, TextureFormat.ARGB32, false);
			var bytes     = texture2D.EncodeToPNG();

			Destroy(texture2D);

			var fs = new System.IO.FileStream(path, overwrite == true ? System.IO.FileMode.Create : System.IO.FileMode.CreateNew);
			var bw = new System.IO.BinaryWriter(fs);

			bw.Write(bytes);

			bw.Close();
			fs.Close();

			ReimportAsset(path);
		}
	}
}
#endif