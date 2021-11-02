/*  This file is part of the "Simple Waypoint System" project by FLOBUK.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from FLOBUK. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using UnityEditor;

/// <summary>
/// FLOBUK about/help/support window.
/// <summary>
public class AboutSWSEditor : EditorWindow
{
    [MenuItem("Window/Simple Waypoint System/About")]
    static void Init()
    {
        AboutSWSEditor aboutWindow = (AboutSWSEditor)EditorWindow.GetWindowWithRect
                (typeof(AboutSWSEditor), new Rect(0, 0, 300, 320), false, "About");
        aboutWindow.Show();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(70);
        GUILayout.Label("Simple Waypoint System", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
   
        GUILayout.BeginHorizontal();
        GUILayout.Space(70);
        GUILayout.Label("by FLOBUK");
        GUILayout.EndHorizontal();        
        GUILayout.Space(20);

        GUILayout.Label("Info", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal();
        GUILayout.Label("Homepage");
        if (GUILayout.Button("Visit", GUILayout.Width(100)))
        {
            Help.BrowseURL("https://flobuk.com");
        }
        GUILayout.EndHorizontal();
		
        GUILayout.BeginHorizontal();
        GUILayout.Label("YouTube");
        if (GUILayout.Button("Visit", GUILayout.Width(100)))
        {
            Help.BrowseURL("https://www.youtube.com/channel/UCCY5CCgf96mbWYXawyW8RuQ");
        }
        GUILayout.EndHorizontal();
		
        GUILayout.Label("Support", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal();
        GUILayout.Label("Script Reference");
        if (GUILayout.Button("Visit", GUILayout.Width(100)))
        {
            Help.BrowseURL("https://flobuk.com/docs/sws/");
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Support Forum");
        if (GUILayout.Button("Visit", GUILayout.Width(100)))
        {
            Help.BrowseURL("https://flobuk.com/forum/");
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Unity Forum");
        if (GUILayout.Button("Visit", GUILayout.Width(100)))
        {
            Help.BrowseURL("https://forum.unity3d.com/threads/115086/");
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.Label("Support me!", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Review Asset");
        if (GUILayout.Button("Visit", GUILayout.Width(100)))
        {
            Help.BrowseURL("https://assetstore.unity.com/packages/slug/2506?aid=1011lGiF&pubref=editor_sws");
        }
        GUILayout.EndHorizontal();
    }
}