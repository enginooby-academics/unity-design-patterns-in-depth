/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Collections.Generic;
using System.Linq;
using InfinityCode.uContext;
using InfinityCode.uContext.Actions;
using InfinityCode.uContext.Integration;
using InfinityCode.uContext.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.Tools
{
    [InitializeOnLoad]
    public static class HierarchyIcons
    {
        private static GameObject prevTarget;
        private static List<Item> prevItems;
        private static Dictionary<int, List<Item>> cache;
        private static bool ehVisible = true;
        private static int ehRightMargin;
        private static int activeID;

        static HierarchyIcons()
        {
            prevItems = new List<Item>();
            HierarchyItemDrawer.Register("HierarchyIcon", DrawHierarchyItem);
        }

        private static void DrawHierarchyItem(HierarchyItem item)
        {
            if (!Prefs.hierarchyIcons) return;

            Event e = Event.current;
            if (Prefs.hierarchyIconsDisplayRule == HierarchyIconsDisplayRule.onHoverWithModifiers && e.modifiers != Prefs.hierarchyIconsModifiers)
            {
                if (!ehVisible)
                {
                    EnhancedHierarchy.SetRightMargin(ehRightMargin);
                    ehVisible = true;
                }
                return;
            }

            if (ehVisible)
            {
                ehRightMargin = EnhancedHierarchy.GetRightMargin();
                EnhancedHierarchy.SetRightMargin(-10000);
                ehVisible = false;
            }

            Rect rect = item.rect;

            if (Prefs.hierarchyIconsDisplayRule != HierarchyIconsDisplayRule.always)
            {
                if (e.type == EventType.Layout)
                {
                    Vector2 p = e.mousePosition;
                    if (p.x < 0 || p.x > rect.xMax || p.y < rect.y || p.y > rect.yMax)
                    {
                        if (item.id == activeID) activeID = -1;
                    }
                    else activeID = item.id;
                }
                else if (activeID != item.id) return;
            }

            if (Prefs.hierarchyIconsDisplayRule != HierarchyIconsDisplayRule.always)
            {
                if (item.gameObject != prevTarget)
                {
                    prevTarget = item.gameObject;
                    UpdateItems(rect, item.gameObject, prevItems);
                }

                if (prevTarget == null) return;

                Rect lastRect = new Rect(rect.xMax, rect.y, 0, rect.height);

                for (int i = prevItems.Count - 1; i >= 0; i--)
                {
                    Item iconItem = prevItems[i];
                    float lastX = iconItem.Draw(lastRect);
                    lastRect.x = lastX;
                }
            }
            else
            {
                Rect lastRect = new Rect(rect.xMax, rect.y, 0, rect.height);

                List<Item> items = GetItemsFromCache(item.gameObject, rect);

                for (int i = items.Count - 1; i >= 0; i--)
                {
                    Item iconItem = items[i];
                    float lastX = iconItem.Draw(lastRect);
                    lastRect.x = lastX;
                }
            }
        }

        private static List<Item> GetItemsFromCache(GameObject target, Rect rect)
        {
            if (target == null) return new List<Item>();

            List<Item> cachedItems;
            if (cache == null) cache = new Dictionary<int, List<Item>>();
            else if (cache.TryGetValue(target.GetInstanceID(), out cachedItems)) return cachedItems;
            cachedItems = new List<Item>();
            UpdateItems(rect, target, cachedItems);
            cache.Add(target.GetInstanceID(), cachedItems);
            return cachedItems;
        }

        private static void ShowAddComponent(Rect hierarchyRect)
        {
            Event e = Event.current;
            Vector2 position = e.mousePosition;
            position.y = hierarchyRect.yMax;
            if (EditorWindow.focusedWindow != null) position += EditorWindow.focusedWindow.position.position;
            else position = HandleUtility.GUIPointToScreenPixelCoordinate(position);

            Vector2 size = Prefs.defaultWindowSize;
            Rect rect = new Rect(position + new Vector2(-size.x / 2, 36), size);

#if !UNITY_EDITOR_OSX
            if (rect.yMax > Screen.currentResolution.height - 10) rect.y -= rect.height - 50;

            if (rect.x < 5) rect.x = 5;
            else if (rect.xMax > Screen.currentResolution.width - 5) rect.x = Screen.currentResolution.width - 5 - rect.width;
#endif

            Selection.activeGameObject = prevTarget;
            AddComponent.ShowAddComponent(rect);
        }

        private static void ShowComponent(Component component, Rect hierarchyRect)
        {
            Event e = Event.current;
            Vector2 position = e.mousePosition;
            position.y = hierarchyRect.yMax;
            if (EditorWindow.focusedWindow != null) position += EditorWindow.focusedWindow.position.position;
            else position = HandleUtility.GUIPointToScreenPixelCoordinate(position);

            Vector2 size = Prefs.defaultWindowSize;
            Rect rect = new Rect(position + new Vector2(-size.x / 2, 36), size);

#if !UNITY_EDITOR_OSX
            if (rect.yMax > Screen.currentResolution.height - 10) rect.y -= rect.height - 50;

            if (rect.x < 5) rect.x = 5;
            else if (rect.xMax > Screen.currentResolution.width - 5) rect.x = Screen.currentResolution.width - 5 - rect.width;
#endif

            ComponentWindow wnd = ComponentWindow.Show(component);
            wnd.position = rect;
        }

        private static void ShowMore(IEnumerable<Component> components, Rect rect)
        {
            GenericMenuEx menu = GenericMenuEx.Start();
            bool useSeparator = false;

            foreach (Component c in components)
            {
                menu.Add(c.GetType().Name, () =>
                {
                    SceneViewManager.OnNextGUI += () => ShowComponent(c, rect);
                    SceneView.RepaintAll();
                });
                useSeparator = true;
            }

            if (useSeparator) menu.AddSeparator("");

            menu.Add("Add Component", () =>
            {
                SceneViewManager.OnNextGUI += () => ShowAddComponent(rect);
                SceneView.RepaintAll();
            });

            menu.Add("Add To Bookmark", () =>
            {
                Bookmarks.Add(prevTarget);
                SceneView.RepaintAll();
            });
            menu.ShowAsContext();
        }

        private static void UpdateItems(Rect rect, GameObject target, List<Item> items)
        {
            items.Clear();

            if (target == null) return;

            Component[] components = target.GetComponents<Component>();
            Item item;

            for (int i = 0; i < Mathf.Min(components.Length, Prefs.hierarchyIconsMaxItems); i++)
            {
                Component component = components[i];
                if (component == null) continue;
                Texture2D thumbnail = AssetPreview.GetMiniThumbnail(component);
                GUIContent content = new GUIContent(
                    thumbnail,
                    ObjectNames.NicifyVariableName(component.GetType().Name)
                );
                if (thumbnail.name == "cs Script Icon" || thumbnail.name == "d_cs Script Icon") GameObjectUtils.GetPsIconContent(content);

                item = new Item(content);
                item.OnClick += () => ShowComponent(component, rect);
                item.OnRightClick += () => ComponentUtils.ShowContextMenu(component);
                items.Add(item);
            }

            int moreItems = components.Length - Prefs.hierarchyIconsMaxItems;

            item = new Item(new GUIContent(moreItems > 0? "+" + moreItems: "...", "More"));
            item.OnClick += () => ShowMore(components.Skip(Prefs.hierarchyIconsMaxItems), rect);
            items.Add(item);
        }

        internal class Item
        {
            public Action OnClick;
            public Action OnRightClick;
            public GUIContent content;

            public Item(GUIContent content)
            {
                this.content = content;
            }

            public float Draw(Rect rect)
            {
                bool useButton = !string.IsNullOrEmpty(content.text);
                rect.xMin -= useButton ? Styles.hierarchyIcon.CalcSize(content).x + 8 : 18;
                GUI.Box(rect, content, Styles.hierarchyIcon);
                Event e = Event.current;
                if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
                {
                    if (e.button == 0)
                    {
                        if (OnClick != null) OnClick();
                        e.Use();
                    }
                    else if (e.button == 1)
                    {
                        if (OnRightClick != null) OnRightClick();
                        e.Use();
                    }
                }

                return rect.x;
            }
        }
    }
}