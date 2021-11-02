/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Text.RegularExpressions;
using InfinityCode.uContext;
using InfinityCode.uContext.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.Windows
{
    [InitializeOnLoad]
    public static class BookmarksTypeFilter
    {
        private static GUIContent filterByTypeContent;

        static BookmarksTypeFilter()
        {
            Bookmarks.OnToolbarMiddle += OnToolbarMiddle;
        }

        private static void OnToolbarMiddle(Bookmarks wnd)
        {
            if (filterByTypeContent == null) filterByTypeContent = EditorGUIUtility.IconContent("FilterByType", "Search by Type");
            if (!GUILayoutUtils.ToolbarButton(filterByTypeContent)) return;

            string[] names = GameObjectUtils.GetTypesDisplayNames();
            string assetType = "";
            string filter = wnd.filter;
            Match match = Regex.Match(filter, @"(:)(\w*)");
            if (match.Success)
            {
                assetType = match.Groups[2].Value.ToUpperInvariant();
                if (assetType == "PREFAB") assetType = "GAMEOBJECT";
            }

            GenericMenuEx menu = GenericMenuEx.Start();
            for (int i = 0; i < names.Length; i++)
            {
                string name = names[i];
                bool isSameType = name.ToUpperInvariant() == assetType;
                menu.Add(name, isSameType, () =>
                {
                    if (!string.IsNullOrEmpty(assetType))
                    {
                        if (isSameType) wnd.filter = Regex.Replace(filter, @"(:)(\w*)", "");
                        else wnd.filter = Regex.Replace(filter, @"(:)(\w*)", ":" + name);
                    }
                    else wnd.filter += ":" + name;
                });
            }
            menu.ShowAsContext();
        }
    }
}