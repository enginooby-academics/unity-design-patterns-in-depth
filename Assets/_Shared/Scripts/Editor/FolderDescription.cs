using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DefaultAsset), isFallback = true)]
public class FolderDescription : Editor {
  // Thanks for all the fish!
  private const string descriptionFilename = ".desc";
  private string description;

  private bool isFolder;

  private void OnEnable() {
    var path = AssetDatabase.GetAssetPath(target);

    if (Directory.Exists(path)) {
      isFolder = true;
      var descriptionPath = Path.Combine(path, descriptionFilename);

      try {
        description = File.ReadAllText(descriptionPath);
      }
      catch (Exception ea) {
        Error(ea);
      } // Print to debug error message only if there was an advanced error
    }
  }

#if UNITY_2019_1_OR_NEWER
  protected override void OnHeaderGUI() {
    base.OnHeaderGUI();

    if (isFolder) DoDescriptionEditor();
  }
#endif

  private void DoDescriptionEditor() {
    GUI.enabled = true;
    EditorGUI.BeginChangeCheck();

    var descriptionContent = new GUIContent(description);

    GUILayout.Label("Description");
    EditorGUILayout.Space(8);

    description =
      EditorGUILayout.TextArea(description, GUILayout.ExpandHeight(false),
        GUILayout.ExpandWidth(true)); // Reverted styling to default

    EditorGUILayout.Space(16);

    if (EditorGUI.EndChangeCheck()) {
      // we get the asset path again instead of caching it in case the folder has moved since OnEnable was called
      var descriptionPath = Path.Combine(AssetDatabase.GetAssetPath(target), descriptionFilename);

      if (!string.IsNullOrEmpty(description)) {
        try {
          File.SetAttributes(descriptionPath, FileAttributes.Normal);
        }
        catch (Exception ea) {
          Error(ea);
        } // Print to debug error message only if there was an advanced error

        File.WriteAllText(descriptionPath, description);

        try {
          File.SetAttributes(descriptionPath, FileAttributes.Normal);
        }
        catch (Exception ea) {
          Error(ea);
        } // Print to debug error message only if there was an advanced error
      }
      else {
        try {
          File.Delete(descriptionPath);
        }
        catch (Exception ea) {
          Error(ea);
        } // Print to debug error message only if there was an advanced error
      }
    }
  }

  private void Error(Exception ea) {
    if (ea is FileNotFoundException) return;
    Debug.LogError(ea.ToString());
  }
}