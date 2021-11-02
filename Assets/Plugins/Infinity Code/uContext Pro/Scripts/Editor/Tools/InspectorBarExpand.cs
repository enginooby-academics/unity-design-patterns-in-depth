/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using InfinityCode.uContext;
using InfinityCode.uContext.Inspector;
using InfinityCode.uContext.UnityTypes;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace InfinityCode.uContextPro.Tools
{
    [InitializeOnLoad]
    public static class InspectorBarExpand
    {
        private static GUIContent _collapseContent;
        private static GUIContent _expandContent;

        private static GUIContent collapseContent
        {
            get
            {
                if (_collapseContent == null) _collapseContent = new GUIContent(Icons.collapse, "Collapse all components");
                return _collapseContent;
            }
        }

        private static GUIContent expandContent
        {
            get
            {
                if (_expandContent == null) _expandContent = new GUIContent(Icons.expand, "Expand all components");
                return _expandContent;
            }
        }


        static InspectorBarExpand()
        {
            InspectorBar.OnDrawBefore += DrawExpand;
        }

        private static void DrawExpand(EditorWindow wnd, Editor[] editors)
        {
            ActiveEditorTracker tracker = InspectorWindowRef.GetTracker(wnd);
            bool isExpanded = false;
            for (int i = 1; i < editors.Length; i++)
            {
                Editor editor = editors[i];
                if (editor is MaterialEditor)
                {
                    if (InternalEditorUtility.GetIsInspectorExpanded(editor.target))
                    {
                        isExpanded = true;
                        break;
                    }
                }
                else if (tracker.GetVisible(i) == 1)
                {
                    isExpanded = true;
                    break;
                }
            }

            GUIContent content = isExpanded ? collapseContent : expandContent;

            if (GUILayout.Button(content, EditorStyles.toolbarButton, GUILayout.Width(25)))
            {
                int v = isExpanded ? 0 : 1;
                for (int i = 1; i < editors.Length; i++)
                {
                    Editor editor = editors[i];
                    if (editor is MaterialEditor)
                    {
                        InternalEditorUtility.SetIsInspectorExpanded(editor.target, !isExpanded);
                    }
                    else tracker.SetVisible(i, v);
                }
            }
        }
    }
}