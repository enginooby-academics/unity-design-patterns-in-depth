using System.Collections;
using UnityEditor;
using UnityEngine;

namespace BrokenVector.RealShortcuts.Utils {
  public class EditorGUINavigationBar : IEnumerable {
    private const float height = 20f;

    private readonly string prefsString;

    private readonly string[] tabNames;


    public EditorGUINavigationBar(string prefsName, string[] tabs, int defaultTab = 0) {
      tabNames = tabs;

      prefsString = prefsName + ".navigationbar.activetab";

      CurrenTabIndex = EditorPrefs.GetInt(prefsString, defaultTab);
      CurrenTabName = tabNames[CurrenTabIndex];
    }

    public int CurrenTabIndex { get; private set; }

    public string CurrenTabName { get; private set; }

    public int TabCount => tabNames.Length;

    public IEnumerator GetEnumerator() => tabNames.GetEnumerator();

    public void DrawNavigationBar() {
      var currCurrentTab = CurrenTabIndex;

      GUILayout.Space(10);
      using (new GUILayout.HorizontalScope()) {
        for (var i = 0; i < TabCount; i++) {
          string styleName;
          if (TabCount == 1)
            styleName = "button";
          else if (i == 0)
            styleName = "buttonLeft";
          else if (i == TabCount - 1)
            styleName = "buttonRight";
          else
            styleName = "buttonMid";

          var style = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle(styleName);
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

      if (CurrenTabIndex != currCurrentTab) {
        CurrenTabIndex = currCurrentTab;
        EditorPrefs.SetInt(prefsString, currCurrentTab);

        CurrenTabName = tabNames[CurrenTabIndex];
      }
    }
  }
}