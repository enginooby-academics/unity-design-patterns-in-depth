using System;
using System.Linq.Expressions;
using com.ootii.Helpers;
using UnityEditor;
using UnityEngine;

namespace com.ootii.Base
{
    // CDL 08/13/2018 = moved this from Editor/MotionController/Setup to Editor/Framework_v1/Base, as it may be useful for
    // building other editor tools
    /// <summary>
    /// Base class for Editor Windows that allows us to define serialized properties on the window
    /// and access them using: FindProperty(x => x.PropertyName). This makes it possible to simply use
    /// EditorGUILayout.PropertyField to display those properties.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ToolWindow<T> : EditorWindow where T : EditorWindow
    {
        // Stores a reference to the Editor Window instance as a Serialized Object; this allows us
        // to access Serialized Properties for the Editor Window.
        protected SerializedObject mSerializedObject;

        // Current window scrollbar position
        protected Vector2 mScrollPosition;

        private GUIStyle mScrollStyle;
        protected GUIStyle ScrollStyle
        {
            get
            {
                if (mScrollStyle == null)                
                    mScrollStyle = new GUIStyle(GUI.skin.scrollView) { padding = new RectOffset(10, 10, 5, 5) };
                

                return mScrollStyle;
            }
        }

        private GUIStyle mCommandButtonStyle;
        protected GUIStyle CommandButtonStyle
        {
            get
            {
                if (mCommandButtonStyle == null)
                {
                   mCommandButtonStyle = new GUIStyle(GUI.skin.button)
                   {
                       stretchWidth = true,
                       fixedHeight = 40f,
                       fontSize = 14,
                       alignment = TextAnchor.MiddleCenter,
                       margin = new RectOffset(10, 10, 10, 10),
                       padding = new RectOffset(20, 20, 10, 10)
                   };
                }

                return mCommandButtonStyle;
            }
        }       

        /// <summary>
        /// Provides access to the target serialized object as its concrete type
        /// </summary>
        protected T mTarget
        {
            get { return (T)mSerializedObject.targetObject; }
        }
        
        protected virtual void OnEnable()
        {
            // set a reference to self as SerializedObject
            if (mSerializedObject == null) mSerializedObject = new SerializedObject(this);
        }

        /// <summary>
        /// Display the primary window header (and optional description)
        /// </summary>
        /// <param name="rTitle"></param>
        /// <param name="rDescription"></param>
        protected virtual void DrawWindowHeader(string rTitle, string rDescription = "")
        {
            GUILayout.Space(5);

            EditorHelper.DrawInspectorTitle(rTitle);
            if (!string.IsNullOrEmpty(rDescription))
            {
                EditorHelper.DrawInspectorDescription(rDescription, MessageType.None);
            }
            GUILayout.Space(5);
        }       
       
        /// <summary>
        /// Find a serialized property on the target object using a lambda expression.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected SerializedProperty FindProperty<TValue>(Expression<Func<T, TValue>> expression)
        {
            return mSerializedObject.FindProperty(ReflectionHelper.GetFieldPath(expression));
        }
    }
}