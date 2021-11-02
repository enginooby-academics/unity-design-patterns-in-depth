/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using InfinityCode.uContext;
using InfinityCode.uContext.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.Windows
{
    [InitializeOnLoad]
    public static class DistanceToolCursor
    {
        private static bool lastPointUnderCursor;

        static DistanceToolCursor()
        {
            DistanceTool.OnUseCursorGUI += OnUseCursor;
            DistanceTool.OnUseCursorSceneGUI += OnUseCursorSceneGUI;
        }

        private static void OnUseCursorSceneGUI(Vector3 prev, bool hasPrev, ref float distance)
        {
            if (lastPointUnderCursor && hasPrev && SceneViewManager.lastGameObjectUnderCursor != null)
            {
                Vector3 p = SceneViewManager.lastWorldPosition;
                Handles.DrawLine(p, prev);
                Handles.Label((p + prev) / 2, DistanceTool.GetDistance(p, prev).ToString("F1"), DistanceTool.distanceStyle);
            }
        }

        private static void OnUseCursor(Vector3 prev, bool hasPrev, ref float distance)
        {
            lastPointUnderCursor = EditorGUILayout.ToggleLeft("Last point is the cursor in Scene View", lastPointUnderCursor);

            if (lastPointUnderCursor)
            {
                if (hasPrev)
                {
                    if (SceneViewManager.lastGameObjectUnderCursor != null)
                    {
                        distance += DistanceTool.GetDistance(SceneViewManager.lastWorldPosition, prev);
                    }
                }

                DistanceTool.isDirty = true;
            }
        }
    }
}