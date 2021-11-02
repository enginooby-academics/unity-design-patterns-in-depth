/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using InfinityCode.uContext;
using InfinityCode.uContext.Tools;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.Tools
{
    [InitializeOnLoad]
    public static class PreviewRestore
    {
        static PreviewRestore()
        {
            KeyManager.KeyBinding focusBinding = KeyManager.AddBinding();
            focusBinding.OnValidate += () =>
            {
                if (!Prefs.preview) return false;
                if (SceneView.lastActiveSceneView == null) return false;
                if (Preview.texture == null) return false;
                if (Event.current.keyCode != KeyCode.F) return false;
                if (Event.current.modifiers != Prefs.previewModifiers) return false;
                return true;
            };
            focusBinding.OnInvoke += FocusActiveItem;

            Preview.OnPostSceneGUI += OnPostSceneGUI;
        }

        private static void FocusActiveItem()
        {
            try
            {
                SceneView view = SceneView.lastActiveSceneView;
                if (view == null) return;

                Preview.PreviewItem activeItem = Preview.activeItem;

                if (activeItem == null) return;
                activeItem.Focus();

                Event.current.Use();
                SceneView.RepaintAll();
            }
            catch (Exception e)
            {
                Log.Add(e);
            }
        }

        private static void OnPostSceneGUI(float width, GUIStyle style, ref float lastY)
        {
            GUI.Label(new Rect(5, 55, width, 20), "F - Set view", style);
        }
    }
}