using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BrokenVector.RealShortcuts.Utils {
  public static class FileUtils {
    public static string GetSelectedPathOrFallback() {
      var path = "Assets";

      foreach (var obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets)) {
        path = AssetDatabase.GetAssetPath(obj);
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
          path = Path.GetDirectoryName(path);
          break;
        }
      }

      return path;
    }

    public static void SetInspectorIcon(Object obj, Texture icon) {
      var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
      var args = new object[] {obj, icon};
      var editorGUIUtilityType = typeof(EditorGUIUtility);
      editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);
      var editorUtilityType = typeof(EditorUtility);
      editorUtilityType.InvokeMember("ForceReloadInspectors", bindingFlags, null, null, null);
    }
  }
}