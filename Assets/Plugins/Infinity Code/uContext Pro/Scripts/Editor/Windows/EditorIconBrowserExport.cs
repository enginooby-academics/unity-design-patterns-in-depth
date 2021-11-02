/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.IO;
using InfinityCode.uContext.Windows;
using UnityEditor;
using UnityEngine;
using Resources = InfinityCode.uContext.Resources;

namespace InfinityCode.uContextPro.Windows
{
    [InitializeOnLoad]
    public static class EditorIconBrowserExport
    {
        static EditorIconBrowserExport()
        {
            EditorIconsBrowser.OnExport += OnExport;
        }

        private static void OnExport(string icon)
        {
            GUIContent content = EditorGUIUtility.IconContent(icon);
            if (content == null || content.image == null) return;

            string filename = EditorUtility.SaveFilePanel("Export " + icon, Application.dataPath, icon, "png");
            if (string.IsNullOrEmpty(filename)) return;

            Texture2D texture = Resources.DuplicateTexture(content.image);
            File.WriteAllBytes(filename, texture.EncodeToPNG());
            EditorUtility.RevealInFinder(filename);
        }
    }
}