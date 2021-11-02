using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using com.ootii.Helpers;

namespace com.ootii.Base
{
    /// <summary>
    /// Base class for custom inspectors (typed). Caches a typed reference to the target
    /// of the serialized object in OnEnable().
    ///
    /// Allows serialized fields to be accessed using
    /// a lambda expression rather than having to hard-code the field name.
    /// </summary>
    /// <example>var myProperty = FindProperty(x => x.myProperty);</example>
    /// <typeparam name="T">Type of the serialized object</typeparam>
    public abstract class BaseInspector<T> : BaseInspector where T : UnityEngine.Object
    {
        // The target serialized object as its concrete type.
        [NonSerialized] protected T mTarget;
        
        // The dirty flag, indicating if changes were made in the Editor.
        [NonSerialized] protected bool mIsDirty;

        // Flag indicating if we need to write unsaved asset changes to disk.
        [NonSerialized] protected bool mSaveChanges;
                
        /// <summary>
        /// This is deliberately left private and non-virtual; override the abstract Initialize() method instead to
        /// set up any required values in derived inspectors.
        /// </summary>
        private void OnEnable()
        {
            if (target == null) { return; }

            mTarget = (T)target;            

            Initialize();
        }

        /// <summary>
        /// Whenever possible, prefer overriding the Draw() method instead of OnInspectorGUI()
        /// </summary>
        public override void OnInspectorGUI()
        {
            Undo.RecordObject(target, "Inspector");
            serializedObject.Update();

            Draw();

            // Sometimes we need to manually flag the object as dirty to ensure the changes get serialized
            if (mIsDirty)
            {
                mIsDirty = false;
                EditorUtility.SetDirty(mTarget);                
            }

            serializedObject.ApplyModifiedProperties();                                  

            // Write all unsaved asset changes to disk (Optional)
            if (mSaveChanges)
            {
                mSaveChanges = false;
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Perform any initialization needed for this inspector. 
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// Find a serialized field on the target object using a lambda expression.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="rExpression"></param>
        /// <returns></returns>
        protected SerializedProperty FindProperty<TValue>(Expression<Func<T, TValue>> rExpression)
        {
            return serializedObject.FindProperty(ReflectionHelper.GetFieldPath(rExpression));
        }

        /// <summary>
        /// Get the tooltip text for the specified field (via Reflection).
        /// </summary>
        /// <param name="rFieldName"></param>
        /// <returns></returns>
        protected string GetTooltip(string rFieldName)
        {
            FieldInfo lFieldInfo = typeof(T).GetField(rFieldName);
            if (lFieldInfo == null) return string.Empty;

            TooltipAttribute[] lAttributes = lFieldInfo.GetCustomAttributes(
                typeof(TooltipAttribute), true) as TooltipAttribute[];
            if (lAttributes == null) { return string.Empty; }

            TooltipAttribute lToolTip = lAttributes.Length > 0 ? lAttributes[0] : null;
            return (lToolTip != null) ? lToolTip.tooltip : string.Empty;
        }
    }

    /// <summary>
    /// Base class for custom inspectors (untyped).
    /// </summary>
    public abstract class BaseInspector : Editor
    {       
        /// <summary>
        /// Whenever possible, prefer overriding the Draw() method instead of OnInspectorGUI()
        /// </summary>
        public override void OnInspectorGUI()
        {
            Undo.RecordObject(target, "Inspector");
            serializedObject.Update();

            Draw();

            serializedObject.ApplyModifiedProperties();                                  
        }

        /// <summary>
        /// Draw the custom inspector. Override and implement this method instead of OnInspectorGUI() whenever possible.
        /// </summary>
        protected abstract void Draw();     

        #region Layout Helper Methods (Static)

        /// <summary>
        /// Draw a group box, using the regular ootii styling
        /// </summary>
        /// <param name="rDrawContents"></param>
        /// <param name="rBoxStyle"></param>
        /// <param name="rDisabled"></param>
        /// <param name="rOptions"></param>
        public static void DrawGroupBox(Action rDrawContents, GUIStyle rBoxStyle = null, bool rDisabled = false, params GUILayoutOption[] rOptions)
        {
            EditorGUI.BeginDisabledGroup(rDisabled);
            try
            {
                GUILayout.BeginVertical(rBoxStyle == null ? EditorHelper.GroupBox : rBoxStyle);
                rDrawContents();
            }
            finally
            {
                GUILayout.EndVertical();
            }

            EditorGUI.EndDisabledGroup();
        }        

        /// <summary>
        /// Draw a horizontal layout group
        /// </summary>
        /// <param name="rDrawContents"></param>
        /// <param name="rDisabled"></param>
        /// <param name="rLayoutOptions"></param>
        public static void DrawHorizontalGroup(Action rDrawContents, bool rDisabled = false, params GUILayoutOption[] rLayoutOptions)
        {
            EditorGUI.BeginDisabledGroup(rDisabled);
            try
            {
                GUILayout.BeginHorizontal(rLayoutOptions);
                rDrawContents();
            }
            finally
            {
                GUILayout.EndHorizontal();
            }   
            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// Draw a group of fields in the inspector
        /// </summary>
        /// <param name="rTitle"></param>
        /// <param name="rDrawContents"></param>
        /// <param name="rLabelStyle"></param>
        /// <param name="rBoxStyle"></param>
        /// <param name="rDisabled"></param>
        public static void DrawInspectorGroup(string rTitle, Action rDrawContents, GUIStyle rLabelStyle = null, GUIStyle rBoxStyle = null,  bool rDisabled = false)
        {
            EditorGUILayout.LabelField(rTitle, rLabelStyle == null ? EditorStyles.boldLabel : rLabelStyle, GUILayout.Height(16f));

            EditorGUI.BeginDisabledGroup(rDisabled);
            try
            {
                EditorGUILayout.BeginVertical(rBoxStyle == null ? EditorHelper.Box : rBoxStyle);
                rDrawContents();
            }
            finally
            {
                EditorGUILayout.EndVertical();
            }
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(5f);
        }

        #endregion Layout Helper Methods (Static)

        #region GUI Styles (Static)

        /// <summary>
        /// Inspector Title label style
        /// </summary>
        private static GUIStyle mTitleStyle;
        public static GUIStyle TitleStyle
        {
            get
            {
                if (mTitleStyle == null)
                {
                    mTitleStyle = new GUIStyle(EditorHelper.Title) { fixedWidth = 300f };
                }

                return mTitleStyle;
            }
        }

        #endregion GUI Styles (Static)
    }
}

