using System.IO;
using UnityEditor;
using UnityEngine;

namespace SWS
{
    [InitializeOnLoad]
    public class ReviewWindowEditor : EditorWindow
    {
        private static Texture2D reviewWindowImage;
        private static string imagePath = "/EditorFiles/Asset_smallLogo.png";


        [MenuItem("Window/Simple Waypoint System/Review Asset", false, 4)]
        static void Init()
        {
            EditorWindow.GetWindowWithRect(typeof(ReviewWindowEditor), new Rect(0, 0, 256, 330), false, "Review Asset");
        }


        void OnGUI()
        {
            if (reviewWindowImage == null)
            {
                var script = MonoScript.FromScriptableObject(this);
                string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(script));
                reviewWindowImage = AssetDatabase.LoadAssetAtPath(path + imagePath, typeof(Texture2D)) as Texture2D;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(reviewWindowImage);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Review Simple Waypoint System", EditorStyles.boldLabel, GUILayout.Width(220));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Please consider leaving a rating on the");
            EditorGUILayout.LabelField("Unity Asset Store. Your support helps me");
            EditorGUILayout.LabelField("to stay motivated and improve this asset.");
            EditorGUILayout.LabelField("Thank you!", GUILayout.Width(220));
            EditorGUILayout.Space();

            if (GUILayout.Button("Review", GUILayout.Height(40)))
            {
                Help.BrowseURL("https://assetstore.unity.com/packages/slug/2506?aid=1011lGiF&pubref=editor_sws");
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("If you are looking for support, please");
            EditorGUILayout.LabelField("head over to the support forum instead.");
        }
    }
}