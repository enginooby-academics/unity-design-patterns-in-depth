/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;

namespace InfinityCode.uContext.Examples
{
    [CustomEditor(typeof(ViewStateReadme))]
    public class ViewStateReadmeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            string message = "To open View Gallery select Window / Infinity Code / uContext / View Gallery or use shortcut CTRL + SHIFT + G (OSX: COMMAND + SHIFT + G).\n\nTo start Quick Preview hold CTRL + SHIFT + Q (OSX: COMMAND + SHIFT + Q) in Scene View.\n\nTo change the shortcuts, open the settings (Window / Infinity Code / uContext / Settings).";
            EditorGUILayout.HelpBox(message, MessageType.Info);
        }
    }
}