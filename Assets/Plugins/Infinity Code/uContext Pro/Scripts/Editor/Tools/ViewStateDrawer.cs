/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Linq;
using InfinityCode.uContext;
using UnityEditor;
using UnityEngine;
using Resources = InfinityCode.uContext.Resources;

namespace InfinityCode.uContextPro.Tools
{
    [InitializeOnLoad]
    public static class ViewStateDrawer
    {
        private static ViewState[] viewStates;
        private static double lastUpdateTime = double.MinValue;
        private static ViewStateWrapper[] states;
        private static GUIContent viewContent;
        private static GUIStyle viewStyle;

        static ViewStateDrawer()
        {
            SceneViewManager.AddListener(DrawViewStates, SceneViewOrder.normal, true);
        }

        private static void DrawViewStates(SceneView sceneView)
        {
            if (!Prefs.showViewStateInScene) return;

            Event e = Event.current;
            if (!e.alt) return;

            if (e.type == EventType.Layout && 
                (viewStates == null || EditorApplication.timeSinceStartup - lastUpdateTime > 10))
            {
                viewStates = Object.FindObjectsOfType<ViewState>();
                lastUpdateTime = EditorApplication.timeSinceStartup;

                if (states == null) states = new ViewStateWrapper[viewStates.Length];
                else if (states.Length < viewStates.Length) states = new ViewStateWrapper[viewStates.Length];
            }

            if (viewStates == null || viewStates.Length == 0) return;

            Camera camera = sceneView.camera;
            Vector3 cameraPosition = camera.transform.position;

            Handles.BeginGUI();

            for (int i = 0; i < viewStates.Length; i++)
            {
                ViewState state = viewStates[i];
                Vector3 position = state.position;
                float magnitude = (position - cameraPosition).magnitude;

                Vector2 point = HandleUtility.WorldToGUIPoint(position);
                states[i] = new ViewStateWrapper
                {
                    state = state,
                    screenPoint = point,
                    position = position,
                    distance = magnitude
                };
            }

            if (viewContent == null || viewContent.image == null)
            {
                viewContent = new GUIContent(Resources.LoadIcon("Eye"));
            }

            if (viewStyle == null)
            {
                viewStyle = new GUIStyle
                {
                    imagePosition = ImagePosition.ImageAbove, 
                    alignment = TextAnchor.MiddleCenter, 
                    normal =
                    {
                        textColor = Color.white
                    }
                };
            }

            Vector2 mousePosition = e.mousePosition;

            foreach (ViewStateWrapper w in states.OrderByDescending(s => s.distance))
            {
                Vector3 vp = camera.WorldToViewportPoint(w.position);
                if (vp.x < 0 || vp.y < 0 || vp.x > 1 || vp.y > 1 || vp.z < 0) continue;

                viewContent.text = w.state.title + "\nDistance: " + w.distance.ToString("F0") + " meters";
                Rect rect = new Rect(w.screenPoint.x - 24, w.screenPoint.y - 24, 48, 48);

                if (rect.Contains(mousePosition)) GUI.color = new Color32(211, 211, 211, 255);

                if (GUI.Button(rect, viewContent, viewStyle))
                {
                    ViewState state = w.state;
                    sceneView.orthographic = state.is2D;
                    sceneView.pivot = state.pivot;
                    sceneView.size = state.size;
                    sceneView.rotation = state.rotation;
                }

                GUI.color = Color.white;

                w.Dispose();
            }

            Handles.EndGUI();
        }

        internal class ViewStateWrapper
        {
            public ViewState state;
            public Vector2 screenPoint;
            public float distance;
            public Vector3 position;

            public void Dispose()
            {
                state = null;
            }
        }
    }
}