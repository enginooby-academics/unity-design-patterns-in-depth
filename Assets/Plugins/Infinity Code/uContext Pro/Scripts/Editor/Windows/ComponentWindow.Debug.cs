/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using System.Linq;
using InfinityCode.uContext;
using InfinityCode.uContext.UnityTypes;
using InfinityCode.uContext.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.Windows
{
    [InitializeOnLoad]
    public static class ComponentWindowDebug
    {
        private static GUIContent debugContent;
        private static GUIContent debugOnContent;
        private static Dictionary<ComponentWindow, Record> records;

        static ComponentWindowDebug()
        {
            ComponentWindow.OnDestroyWindow += OnDestroyWindow;
            ComponentWindow.OnDrawHeader += OnDrawHeader;
            ComponentWindow.OnDrawContent += OnDrawContent;
            ComponentWindow.OnValidateEditor += OnValidateEditor;

            records = new Dictionary<ComponentWindow, Record>();
        }

        private static void CacheSerializedObject(ComponentWindow window, Record record)
        {
            record.serializedObject = new SerializedObject(window.component);
            record.serializedObject.Update();
            record.searchableProperties = new List<SearchableProperty>();

            SerializedProperty p = record.serializedObject.GetIterator();
            if (!p.Next(true)) return;

            do
            {
                record.searchableProperties.Add(new SearchableProperty(p));
            }
            while (p.NextVisible(false));

            if (!string.IsNullOrEmpty(record.filter)) UpdateFilteredItems(record);
        }

        private static void DrawSerializedObject(ComponentWindow window, Record record)
        {
            if (record.serializedObject == null) CacheSerializedObject(window, record);
            record.serializedObject.Update();

            if (record.filteredItems == null)
            {
                SerializedProperty p = record.serializedObject.GetIterator();

                if (!p.Next(true)) return;

                do
                {
                    EditorGUILayout.PropertyField(p);
                } while (p.NextVisible(false));
            }
            else
            {
                foreach (SearchableProperty item in record.filteredItems) EditorGUILayout.PropertyField(record.serializedObject.FindProperty(item.name));
            }

            record.serializedObject.ApplyModifiedProperties();
        }

        private static void OnDestroyWindow(ComponentWindow window)
        {
            Record record;
            if (!records.TryGetValue(window, out record)) return;

            record.Dispose();
            records.Remove(window);
        }

        private static bool OnDrawContent(ComponentWindow window)
        {
            Record record;
            if (!records.TryGetValue(window, out record)) return false;
            if (!record.isDebug) return false;

            EditorGUI.BeginChangeCheck();
            record.filter = EditorGUILayoutEx.ToolbarSearchField(record.filter);
            if (EditorGUI.EndChangeCheck()) UpdateFilteredItems(record);

            DrawSerializedObject(window, record);

            return true;
        }

        private static void OnDrawHeader(ComponentWindow window, Rect rect)
        {
            if (debugContent == null) debugContent = new GUIContent(Icons.debug, "Debug");
            if (debugOnContent == null) debugOnContent = new GUIContent(Icons.debugOn, "Debug");

            Record record;
            if (!records.TryGetValue(window, out record))
            {
                records[window] = record = new Record();
            }

            if (GUI.Button(new Rect(rect.width - 36, rect.y, 16, 16), record.isDebug ? debugOnContent : debugContent, Styles.transparentButton))
            {
                ToggleDebugMode(window, record);
            }
        }

        private static bool OnValidateEditor(ComponentWindow window)
        {
            Record record;
            if (records.TryGetValue(window, out record)) return record.isDebug;
            return false;
        }

        private static void ToggleDebugMode(ComponentWindow window, Record record)
        {
            record.isDebug = !record.isDebug;
            if (record.isDebug)
            {
                window.FreeEditor();
                CacheSerializedObject(window, record);
            }
            else
            {
                record.serializedObject = null;

                if (window.component is Terrain) window.TryRestoreTerrainEditor();
                else window.InitEditor();
            }
            window.allowInitEditor = !record.isDebug;
        }

        private static void UpdateFilteredItems(Record record)
        {
            if (string.IsNullOrEmpty(record.filter))
            {
                record.filteredItems = null;
                return;
            }

            string assetType;
            string pattern = SearchableItem.GetPattern(record.filter, out assetType);
            record.filteredItems = record.searchableProperties.Where(p => p.UpdateAccuracy(pattern) > 0).OrderByDescending(p => p.accuracy).ToList();
        }

        public class Record
        {
            public bool isDebug;
            public SerializedObject serializedObject;
            public string filter = "";
            public List<SearchableProperty> searchableProperties;
            public List<SearchableProperty> filteredItems;

            public void Dispose()
            {
                serializedObject = null;
                searchableProperties = null;
                filteredItems = null;
            }
        }

        public class SearchableProperty : SearchableItem
        {
            public string name;
            private string[] _search;
            private string displayName;

            public SearchableProperty(SerializedProperty prop)
            {
                name = prop.name;
                displayName = prop.displayName;
            }

            protected override string[] GetSearchStrings()
            {
                if (_search == null) _search = new[] { displayName };
                return _search;
            }
        }
    }
}