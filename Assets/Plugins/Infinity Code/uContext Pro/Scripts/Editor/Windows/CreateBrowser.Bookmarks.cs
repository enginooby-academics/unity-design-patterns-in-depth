/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using InfinityCode.uContext.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.Tools
{
    [InitializeOnLoad]
    public static class CreateBrowserBookmarks
    {
        static CreateBrowserBookmarks()
        {
            CreateBrowser.OnInitProviders += OnInitProviders;
        }

        private static IEnumerable<CreateBrowser.Provider> OnInitProviders()
        {
            return new []
            {
                new BookmarkProvider()
            };
        }

        public class BookmarkProvider : CreateBrowser.Provider
        {
            public override float order
            {
                get { return -1; }
            }

            public override string title
            {
                get { return "Bookmarks"; }
            }

            public override void Cache()
            {
                items = new List<CreateBrowser.Item>();

                string goType = typeof(GameObject).AssemblyQualifiedName;
                foreach (var item in Bookmarks.items)
                {
                    if (item.type == goType)
                    {
                        if (!item.isMissed && item.isProjectItem)
                        {
                            string path = AssetDatabase.GetAssetPath(item.target);
                            if (item.title.Length == 0) continue;

                            items.Add(new CreateBrowser.PrefabItem(item.title + ".prefab", path));
                        }
                    }
                }
            }

            public override void Filter(string pattern, List<CreateBrowser.Item> filteredItems)
            {

            }
        }
    }
}