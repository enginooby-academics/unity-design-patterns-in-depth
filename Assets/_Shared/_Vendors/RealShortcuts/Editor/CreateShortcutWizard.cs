using System;
using System.IO;
using BrokenVector.RealShortcuts.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BrokenVector.RealShortcuts {
  public class CreateShortcutWizard : ScriptableWizard {
    private string name = "";

    private string path = "";

    private EditorGUINavigationBar tabs;

    protected void OnWizardCreate() {
      if (!path.StartsWith("Assets", StringComparison.OrdinalIgnoreCase))
        return;

      var asset = CreateInstance<Shortcut>();
      asset.TargetPath = path;

      var shortcutPath = FileUtils.GetSelectedPathOrFallback() + "/" + name + ".asset";
      AssetDatabase.CreateAsset(asset, shortcutPath);
      AssetDatabase.SaveAssets();

      var shortcut = AssetDatabase.LoadAssetAtPath(shortcutPath, typeof(Object));
      FileUtils.SetInspectorIcon(shortcut, Constants.FileIcon);

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }

    protected void OnWizardUpdate() {
      helpString = "Choose a file or folder as target for your shortcut.";

      if (AssetDatabase.LoadAssetAtPath(FileUtils.GetSelectedPathOrFallback() + "/" + name + ".asset",
            typeof(Object)) != null)
        errorString = "A file with this name already exists! File will be overwritten.";

      if (!string.IsNullOrEmpty(path) && !path.StartsWith("Assets", StringComparison.OrdinalIgnoreCase))
        errorString = "Invalid path! The folder/file has to be inside the Assets folder.";
    }


    [MenuItem("Assets/Create/Shortcut", false, 21)]
    public static void CreateWizard() {
      DisplayWizard<CreateShortcutWizard>("Create Shortcut", "Create");
    }

    protected override bool DrawWizardGUI() {
      if (tabs == null)
        tabs = new EditorGUINavigationBar("brokenvector.realshortcuts", new[] {"File", "Folder"}, 1);

      tabs.DrawNavigationBar();

      EditorGUILayout.BeginVertical("ShurikenEffectBg", GUILayout.MaxHeight(150.0f));
      {
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Select " + tabs.CurrenTabName)) {
          switch (tabs.CurrenTabIndex) {
            case 0:
              path = EditorUtility.OpenFilePanel("Select target file", FileUtils.GetSelectedPathOrFallback(), "");
              break;
            case 1:
            default:
              path = EditorUtility.OpenFolderPanel("Select target folder", FileUtils.GetSelectedPathOrFallback(), "");
              break;
          }

          var idx = path.IndexOf("Assets/", StringComparison.OrdinalIgnoreCase);
          if (idx >= 0)
            path = path.Substring(idx);

          if (name == "") name = Path.GetFileNameWithoutExtension(path);
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginHorizontal();
        {
          EditorGUILayout.LabelField("Name:", GUILayout.Width(50));
          name = GUILayout.TextField(name, 30);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
      }
      EditorGUILayout.EndVertical();

      GUILayout.Space(10);
      GUILayout.FlexibleSpace();

      EditorGUILayout.HelpBox(
        "You can choose every folder or file in you project to link to.\nThe name will be the name of the shortcut (can be changed later).",
        MessageType.Info);

      return true;
    }
  }
}