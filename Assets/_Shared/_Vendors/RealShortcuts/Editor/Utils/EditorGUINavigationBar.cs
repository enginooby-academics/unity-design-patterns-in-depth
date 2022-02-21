using UnityEngine;
using System.Collections;
using UnityEditor;

namespace BrokenVector.RealShortcuts.Utils
{
    public class EditorGUINavigationBar : IEnumerable
    {

        private const float height = 20f;

        private string[] tabNames;
        private string prefsString;

        private int currentTabIndex;
        private string currentTabName;

        public int CurrenTabIndex { get { return currentTabIndex; } }
        public string CurrenTabName { get { return currentTabName; } }
        public int TabCount { get { return tabNames.Length; } }


        public EditorGUINavigationBar(string prefsName, string[] tabs, int defaultTab = 0)
        {
            this.tabNames = tabs;

            prefsString = prefsName + ".navigationbar.activetab";

            currentTabIndex = EditorPrefs.GetInt(prefsString, defaultTab);
            currentTabName = tabNames[currentTabIndex];
        }

        public void DrawNavigationBar()
        {
            var currCurrentTab = currentTabIndex;

            GUILayout.Space(10);
            using (new GUILayout.HorizontalScope())
            {
                for (int i = 0; i < TabCount; i++)
                {
                    string styleName;
                    if (TabCount == 1)
                        styleName = "button";
                    else if (i == 0)
                        styleName = "buttonLeft";
                    else if (i == TabCount - 1)
                        styleName = "buttonRight";
                    else
                        styleName = "buttonMid";

                    GUIStyle style = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle(styleName);
                    var heightBckp = style.fixedHeight;
                    style.fixedHeight = height;

                    var colorBckp = GUI.backgroundColor;
                    if (i == currCurrentTab)
                        GUI.backgroundColor = Color.gray;

                    if (GUILayout.Button(tabNames[i], style))
                        currCurrentTab = i;

                    style.fixedHeight = heightBckp;
                    GUI.backgroundColor = colorBckp;
                }
            }
            
            if (currentTabIndex != currCurrentTab)
            {
                currentTabIndex = currCurrentTab;
                EditorPrefs.SetInt(prefsString, currCurrentTab);

                currentTabName = tabNames[currentTabIndex];
            }
        }

        public IEnumerator GetEnumerator()
        {
            return tabNames.GetEnumerator();
        }

    }
}
