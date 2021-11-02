/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Reflection;
using InfinityCode.uContext;
using InfinityCode.uContext.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.Tools
{
    [InitializeOnLoad]
    public static class QuickAccessExtraTypes
    {
        static QuickAccessExtraTypes()
        {
            QuickAccess.OnInvokeExternal += OnInvokeExternal;
        }

        private static void InvokeMenuItem(QuickAccessItem item)
        {
            string menuPath = item.settings[0];
            if (string.IsNullOrEmpty(menuPath)) return;
            EditorApplication.ExecuteMenuItem(menuPath);

            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView != null) sceneView.Focus();
        }

        private static void InvokeScriptableObject(QuickAccessItem item)
        {
            int activeWindowIndex = QuickAccess.activeWindowIndex;
            QuickAccess.CloseActiveWindow();

            if (activeWindowIndex == QuickAccess.invokeItemIndex) return;
            if (item.scriptableObject == null) return;

            Rect rect = new Rect();
            Vector2 pos = new Vector2(QuickAccess.invokeItemRect.xMax, QuickAccess.invokeItemRect.y + PinAndClose.HEIGHT + 40);
#if !UNITY_2020_1_OR_NEWER
            pos += SceneView.lastActiveSceneView.position.position;
#endif
            rect.position = GUIUtility.GUIToScreenPoint(pos);
            rect.size = Prefs.defaultWindowSize;
            AutoSize autoSize =  AutoSize.top;

            if (rect.center.y > Screen.currentResolution.height / 2)
            {
                autoSize = AutoSize.bottom;
                rect.y -= rect.size.y - PinAndClose.HEIGHT + 8;
            }

            ObjectWindow window = ObjectWindow.ShowPopup(new[] {item.scriptableObject}, rect);
            window.closeOnLossFocus = false;
            
            window.adjustHeight = autoSize;
            QuickAccess.activeWindow = window;
            QuickAccess.activeWindowIndex = QuickAccess.invokeItemIndex;
        }

        private static void InvokeStaticMethod(QuickAccessItem item)
        {
            if (item.settings.Length < 2) return;

            string className = item.settings[0];
            string methodName = item.settings[1];
            if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(methodName)) return;

            Type type = Type.GetType(className);
            if (type == null) return;

            MethodInfo method = type.GetMethod(methodName, Reflection.StaticLookup);
            if (method == null) return;

            method.Invoke(null, new object[0]);

            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView != null) sceneView.Focus();
        }

        private static void OnInvokeExternal(QuickAccessItem item)
        {
            if (item.type == QuickAccessItemType.staticMethod) InvokeStaticMethod(item);
            else if (item.type == QuickAccessItemType.menuItem) InvokeMenuItem(item);
            else if (item.type == QuickAccessItemType.scriptableObject) InvokeScriptableObject(item);
        }
    }
}