using UnityEditor;
using UnityEngine;

namespace BrokenVector.RealShortcuts {
  [CustomEditor(typeof(Shortcut))]
  public class ShortcutInspector : Editor {
#if UNITY_5_4_OR_NEWER
#else
        static ShortcutInspector()
        {
            EditorApplication.projectWindowItemOnGUI += WindowItemOnGUI;
        }
#endif

    public override void OnInspectorGUI() {
      GUILayout.Space(10);

      GUI.skin.label.fontSize = 20;
      GUILayout.Label("Shortcut");
      GUI.skin.label.fontSize = 0;

      GUILayout.Space(20);

      var shortcut = (Shortcut) target;

      EditorGUILayout.BeginHorizontal();
      {
        EditorGUILayout.LabelField("Target:", GUILayout.Width(50));
        EditorGUILayout.TextArea(shortcut.TargetPath);
      }
      EditorGUILayout.EndHorizontal();

      GUILayout.Space(20);

      EditorGUILayout.BeginHorizontal();
      {
        if (GUILayout.Button("Open")) OpenShortcut(shortcut);
        if (GUILayout.Button("Remove")) AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(target));
      }
      EditorGUILayout.EndHorizontal();
    }

#if UNITY_5_4_OR_NEWER
#else
        private static void WindowItemOnGUI(string guid, Rect rect)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            Shortcut shortcut = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Shortcut)) as Shortcut;
            if (shortcut == null)
                return;

            rect.width = rect.height;
            GUI.DrawTexture(rect, Constants.FileIcon);

            if (Event.current.isMouse && Event.current.clickCount == 2 && Selection.activeObject is Shortcut)
            {
                OpenShortcut(shortcut);
            }
        }
#endif

    public static void OpenShortcut(Shortcut shortcut) {
      var obj = AssetDatabase.LoadAssetAtPath(shortcut.TargetPath, typeof(Object));

      Selection.activeObject = obj;
      EditorGUIUtility.PingObject(obj);
    }
  }
}