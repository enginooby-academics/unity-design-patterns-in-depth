/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Linq;
using InfinityCode.uContext;
using InfinityCode.uContext.TransformEditorTools;
using InfinityCode.uContext.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.TransformEditorTools
{
    public class AlignTool: TransformEditorTool
    {
        private GUIStyle rightLabel;

        public override void Draw()
        {
            Transform[] transforms = TransformEditorWindow.GetTransforms();
            if (transforms.Length <= 1) return;

            if (rightLabel == null)
            {
                rightLabel = new GUIStyle(EditorStyles.label)
                {
                    alignment = TextAnchor.MiddleRight
                };
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Align", GUILayout.ExpandWidth(false));
            GUILayout.Label("Distribute", rightLabel);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("X", GUILayout.ExpandWidth(false));
            if (GUILayout.Button("Min"))
            {
                GameObjectUtils.Align(Selection.gameObjects, 0, 1, 0, 0);
            }
            if (GUILayout.Button("Center"))
            {
                GameObjectUtils.Align(Selection.gameObjects, 1, 1, 0, 0);
            }
            if (GUILayout.Button("Max"))
            {
                GameObjectUtils.Align(Selection.gameObjects, 2, 1, 0, 0);
            }

            GUILayout.Space(10);

            if (GUILayout.Button("X"))
            {
                GameObjectUtils.Distribute(Selection.gameObjects, 1, 0, 0);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Y", GUILayout.ExpandWidth(false));
            if (GUILayout.Button("Min"))
            {
                GameObjectUtils.Align(Selection.gameObjects, 0, 0, 1, 0);
            }
            if (GUILayout.Button("Center"))
            {
                GameObjectUtils.Align(Selection.gameObjects, 1, 0, 1, 0);
            }
            if (GUILayout.Button("Max"))
            {
                GameObjectUtils.Align(Selection.gameObjects, 2, 0, 1, 0);
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Y"))
            {
                GameObjectUtils.Distribute(Selection.gameObjects, 0, 1, 0);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Z", GUILayout.ExpandWidth(false));
            if (GUILayout.Button("Min"))
            {
                GameObjectUtils.Align(Selection.gameObjects, 0, 0, 0, 1);
            }
            if (GUILayout.Button("Center"))
            {
                GameObjectUtils.Align(Selection.gameObjects, 1, 0, 0, 1);
            }
            if (GUILayout.Button("Max"))
            {
                GameObjectUtils.Align(Selection.gameObjects, 2, 0, 0, 1);
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Z"))
            {
                GameObjectUtils.Distribute(Selection.gameObjects, 0, 0, 1);
            }

            EditorGUILayout.EndHorizontal();
        }

        public override void Init()
        {
            _content = new GUIContent(Styles.isProSkin? Icons.align: Icons.alignDark, "Align & Distribute");
        }

        public override bool Validate()
        {
            return Selection.gameObjects.Count(g => g.scene.name != null) > 1;
        }
    }
}