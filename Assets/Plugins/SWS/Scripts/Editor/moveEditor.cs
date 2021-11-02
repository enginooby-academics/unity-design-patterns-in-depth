/*  This file is part of the "Simple Waypoint System" project by FLOBUK.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from FLOBUK. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using UnityEditor;

namespace SWS
{
    /// <summary>
    /// Serves as template for movement script editor inspectors.
    /// <summary>
    //[CustomEditor(typeof(...))]
    public class moveEditor : Editor
    {
        //define Serialized Objects we want to use/control
        //this will be our serialized reference to the inspected script
        public SerializedObject m_Object;

        //inspector scrollbar x/y position, modified by mouse input
        public Vector2 scrollPosEvents;
        //whether events settings menu should be visible
        public bool showEventSetup = false;


        //called whenever this inspector window is loaded 
        public virtual void OnEnable()
        {
            //we create a reference to our script object by passing in the target
            m_Object = new SerializedObject(target);
        }


        //returns PathManager component for later use
        public virtual PathManager GetPathTransform()
        {
            //get pathContainer from serialized property and return its PathManager component
            return m_Object.FindProperty("pathContainer").objectReferenceValue as PathManager;
        }


        //called whenever the inspector gui gets rendered
        public override void OnInspectorGUI()
        {
            //this pulls the relative variables from unity runtime and stores them in the object
            m_Object.Update();

            //show default variables in inspector
            DrawDefaultInspector();

            EditorGUILayout.Space();
            //draw message options
            EventSettings();

            //we push our modified variables back to our serialized object
            m_Object.ApplyModifiedProperties();
        }


        public virtual void EventSettings()
        {
            //path is set and boolean for displaying events settings is true
            if (showEventSetup)
            {
                //draw button for hiding events
                if (GUILayout.Button("Hide UnityEvents"))
                    showEventSetup = false;

                //begin a scrolling view inside GUI, pass in Vector2 scroll position 
                scrollPosEvents = EditorGUILayout.BeginScrollView(scrollPosEvents, GUILayout.Height(255));

                EditorGUILayout.PropertyField(m_Object.FindProperty("movementStart"));
                EditorGUILayout.PropertyField(m_Object.FindProperty("movementChange"));
                EditorGUILayout.PropertyField(m_Object.FindProperty("movementEnd"));

                //ends the scrollview defined above
                EditorGUILayout.EndScrollView();
            }
            else
            {
                //draw button to toggle events
                if (GUILayout.Button("Show UnityEvents"))
                    showEventSetup = true;
            }
        }
    }
}