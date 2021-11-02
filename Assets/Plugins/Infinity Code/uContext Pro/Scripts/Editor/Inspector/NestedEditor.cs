/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Collections.Generic;
using InfinityCode.uContext;
using InfinityCode.uContext.Inspector;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InfinityCode.uContextPro.Inspector
{
    [InitializeOnLoad]
    public class NestedEditor
    {
#if UNITY_2021_1_OR_NEWER
        private const bool TOGGLE_ON_LABEL_CLICK = false;
#else 
        private const bool TOGGLE_ON_LABEL_CLICK = true;
#endif

        private static GUIStyle _backgroundStyle;
        private static Dictionary<int, NestedEditor> editorCache = new Dictionary<int, NestedEditor>();
        private static float labelWidth;

        private Editor editor;

        private static GUIStyle backgroundStyle
        {
            get
            {
                if (_backgroundStyle == null)
                {
                    _backgroundStyle = new GUIStyle(GUI.skin.box)
                    {
                        margin =
                        {
                            top = 0,
                            bottom = 0
                        }
                    };
                }

                return _backgroundStyle;
            }
        }

        private Object target
        {
            get { return editor != null ? editor.target : null; }
        }

        static NestedEditor()
        {
            ObjectFieldDrawer.OnGUIBefore += OnGUIBefore;
            ObjectFieldDrawer.OnGUIAfter += OnGUIAfter;
            CompilationPipeline.compilationStarted += OnCompilationStarted;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        public NestedEditor(Object obj)
        {
            try
            {
                editor = Editor.CreateEditor(obj);
            }
            catch (Exception exception)
            {
                Log.Add(exception);
            }
        }

        private void Dispose()
        {
            if (editor != null)
            {
                Object.DestroyImmediate(editor);
                editor = null;
            }
        }

        private void Draw()
        {
            bool enabled = GUI.enabled;
            GUI.enabled = (editor.target.hideFlags & HideFlags.NotEditable) != HideFlags.NotEditable;

            EditorGUI.indentLevel++;
            GUILayout.BeginVertical(backgroundStyle);

            if (editor != null)
            {
                try
                {
                    editor.OnInspectorGUI();
                }
                catch (ExitGUIException)
                {

                }
                catch (Exception exception)
                {
                    Log.Add(exception);
                }
            }

            GUILayout.EndVertical();
            EditorGUI.indentLevel--;
            GUI.enabled = enabled;
        }

        private static void DrawNestedEditor(Rect area, SerializedProperty property)
        {
            Object obj = property.objectReferenceValue;
            if (!ValidateTarget(obj)) return;

            area.xMin -= GUILayoutUtils.nestedEditorMargin;

            bool expanded = property.isExpanded;
            property.isExpanded = EditorGUI.Foldout(area, expanded, GUIContent.none, TOGGLE_ON_LABEL_CLICK);
            if (!expanded) return;

            try
            {
                NestedEditor editor = GetNestedEditor(property);
                if (editor != null) editor.Draw();
            }
            catch (ExitGUIException)
            {
            }
            catch (Exception exception)
            {
                Log.Add(exception);
            }
        }

        private static void FreeCache()
        {
            if (editorCache != null)
            {
                foreach (KeyValuePair<int, NestedEditor> pair in editorCache)
                {
                    NestedEditor nestedEditor = pair.Value;
                    if (nestedEditor.editor != null) Object.DestroyImmediate(nestedEditor.editor);
                }
            }
        }

        private static NestedEditor GetNestedEditor(SerializedProperty property, bool createIfMissed = true)
        {
            NestedEditor editor;
            Object obj = property.objectReferenceValue;
            if (obj == null) return null;

            int id = obj.GetInstanceID();
            editorCache.TryGetValue(id, out editor);
            if (!createIfMissed) return editor;

            if (editor == null || editor.target != obj)
            {
                if (editor != null) editor.Dispose();
                editor = new NestedEditor(obj);
                editorCache[id] = editor;
            }

            return editor;
        }

        private static void OnCompilationStarted(object obj)
        {
            FreeCache();
        }

        private static void OnGUIAfter(Rect area, SerializedProperty property, GUIContent label)
        {
            if (!Prefs.nestedEditors) return;

            DrawNestedEditor(area, property);
            EditorGUIUtility.labelWidth = labelWidth;
        }

        private static void OnGUIBefore(Rect area, SerializedProperty property, GUIContent label)
        {
            if (!Prefs.nestedEditors) return;

            labelWidth = EditorGUIUtility.labelWidth;
            if (!EditorGUIUtility.hierarchyMode) EditorGUIUtility.labelWidth += EditorGUI.indentLevel * 15;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            FreeCache();
        }

        private static bool ValidateTarget(Object target)
        {
            if (target == null) return false;
            if (target is Texture) return false;
            if (target is Material) return false;
            if (target is Shader) return false;
            if (target is GameObject) return false;
            return true;
        }
    }
}