/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using InfinityCode.uContext;
using InfinityCode.uContext.Tools;
using InfinityCode.uContext.TransformEditorTools;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.TransformEditorTools
{
    public class BoundsTool : TransformEditorTool
    {
        public override void Draw()
        {
            GUILayout.Label("Bounds");

            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 85;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Vector3Field("Center", SelectionBoundsManager.bounds.center);
            EditorGUILayout.Vector3Field("Size", SelectionBoundsManager.bounds.size);
            EditorGUI.EndDisabledGroup();

            EditorGUIUtility.labelWidth = labelWidth;
        }

        public override void Init()
        {
            _content = new GUIContent(Styles.isProSkin ? Icons.bounds : Icons.boundsDark, "Bounds");
        }

        public override void OnDisable()
        {
            SelectionSize.SetState(false);
        }

        public override void OnEnable()
        {
            SelectionSize.SetState(true);
        }

        public override bool Validate()
        {
            return SelectionBoundsManager.hasBounds;
        }
    }
}