/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Linq;
using System.Text.RegularExpressions;
using InfinityCode.uContext.Tools;
using InfinityCode.uContext.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.Tools
{
    [InitializeOnLoad]
    public static class MassRename
    {
        private static int index;

        static MassRename()
        {
            Rename.OnMassRename += OnMassRename;
        }

        private static void DrawTokens(InputDialog dialog)
        {
            EditorGUILayout.LabelField("Tokens:");
            EditorGUILayout.LabelField("{C} - counter");
            EditorGUILayout.LabelField("{S} - sibling index");
            EditorGUILayout.LabelField("{START:LEN} - part of the original name");
        }

        private static void OnMassRename()
        {
            InputDialog dialog = InputDialog.Show("Enter a new GameObject name", Rename.gameObjects[0].name, OnRename);
            dialog.OnClose += Rename.OnDialogClose;
            dialog.OnDrawExtra += DrawTokens;
            dialog.minSize = new Vector2(dialog.minSize.x, 130);
            index = 1;
        }

        private static void OnRename(string name)
        {
            if (Rename.gameObjects == null || Rename.gameObjects.Length == 0) return;

            Undo.RecordObjects(Rename.gameObjects, "Rename GameObjects");
            foreach (GameObject go in Rename.gameObjects.Where(g => g.scene.name != null).OrderBy(g => g.transform.GetSiblingIndex()))
            {
                go.name = ReplaceTokens(go, name);
            }

            Rename.gameObjects = null;
        }

        private static string ReplaceTokens(GameObject go, string name)
        {
            name = Regex.Replace(name, @"{[\w\d:-]+}", delegate (Match match)
            {
                string v = match.Value.Trim('{', '}');
                if (v == "C")
                {
                    int i = index++;
                    return i.ToString();
                }

                if (v == "S") return go.transform.GetSiblingIndex().ToString();

                string[] ss = v.Split(':');
                if (ss.Length >= 2)
                {
                    string original = go.name;
                    int start = 0, len = 0;

                    if (!string.IsNullOrEmpty(ss[0]) && !int.TryParse(ss[0], out start)) return "";

                    if (start < 0) start = original.Length + start;

                    if (!string.IsNullOrEmpty(ss[1]))
                    {
                        if (!int.TryParse(ss[1], out len)) return "";
                        if (len < 0) len = original.Length - start + len;
                    }
                    else len = original.Length - start;

                    if (original.Length <= start) return "";
                    if (original.Length < start + len) len = original.Length - start;
                    return original.Substring(start, len);
                }

                return "";
            });
            return name;
        }
    }
}