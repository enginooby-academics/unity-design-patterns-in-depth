/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using InfinityCode.uContext;
using InfinityCode.uContext.UnityTypes;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.Tools
{
    [InitializeOnLoad]
    public static class HierarchyIconSelector
    {
        static HierarchyIconSelector()
        {
            HierarchyItemDrawer.Register("HierarchyIconSelector", OnHierarchyGUI);
        }

        private static void OnHierarchyGUI(HierarchyItem item)
        {
            if (item.gameObject == null || !item.hovered) return;

            Event e = Event.current;
            if (e.type != EventType.MouseUp || e.button != 1) return;

            Rect r = new Rect(item.rect.x, item.rect.y, 16, 16);
            if (!r.Contains(e.mousePosition)) return;

            IconSelectorRef.ShowAtPosition(item.target, r, true);
            e.Use();
        }
    }
}