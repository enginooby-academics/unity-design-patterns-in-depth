#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Enginoobz.Utils {
  public static class EditorUtils {
    public static void StopPlayMode() {
      EditorApplication.isPlaying = false;
      // Application.Quit();
    }

    public static bool IsInteger(this SerializedProperty property) =>
      property.propertyType == SerializedPropertyType.Integer;

    public static bool DisplayDialog(string message, string title = "", string ok = "Yes", string cancel = "No") =>
      EditorUtility.DisplayDialog(title, message, ok, cancel);

    public static List<T> GetAssetsWithScript<T>(string path) where T : MonoBehaviour {
      var assets = new List<T>();
      // UTIL: AssetUtils - FindPrefabs
      var guids = AssetDatabase.FindAssets("t:Prefab", new[] {path});

      for (var i = 0; i < guids.Length; i++) {
        // UTIL: AssetUtils - GetAssetByGUID
        var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
        var assetGO = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        // UTIL: List add if contain component
        if (assetGO.TryGetComponent(out T component)) assets.Add(component);
      }

      return assets;
    }
  }
}
#endif