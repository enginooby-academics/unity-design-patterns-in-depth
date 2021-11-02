/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using InfinityCode.uContext;
using InfinityCode.uContext.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.Windows
{
    [InitializeOnLoad]
    public static class DistanceToolPick
    {
        private static DistanceTool wnd;

        static DistanceToolPick()
        {
            DistanceTool.OnPickStarted += OnPickStarted;
        }

        private static void OnPickStarted(DistanceTool tool)
        {
            wnd = tool;
            if (SceneView.lastActiveSceneView != null) SceneView.lastActiveSceneView.Focus();
            SceneViewManager.AddListener(OnSceneView);
        }

        private static void OnSceneView(SceneView sceneView)
        {
            Event e = Event.current;
            if (DistanceTool.pickTarget == null || wnd == null)
            {
                DistanceTool.pickTarget = null;
                SceneViewManager.RemoveListener(OnSceneView);
                return;
            }

            if (e.type == EventType.Repaint && SceneViewManager.lastGameObjectUnderCursor != null)
            {
                Vector3 p = SceneViewManager.lastWorldPosition;
                float d = (DistanceTool.pickTarget.point - p).sqrMagnitude;
                DistanceTool.pickTarget.point = p;
                if (d > 0) wnd.Repaint();
            }

            if (e.type != EventType.KeyDown || e.keyCode != KeyCode.Return && e.keyCode != KeyCode.KeypadEnter) return;

            wnd.Repaint();
            DistanceTool.pickTarget = null;
            wnd = null;
            SceneViewManager.RemoveListener(OnSceneView);
        } 
    }
}