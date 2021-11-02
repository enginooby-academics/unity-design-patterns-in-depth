/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using InfinityCode.uContext.FloatToolbar;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace InfinityCode.uContextPro.Tools
{
    [InitializeOnLoad]
    public static class ObjectToolbarRelatedComponents
    {
        static ObjectToolbarRelatedComponents()
        {
            ObjectToolbar.OnGetRelatedComponents += OnGetRelatedComponents;
        }

        private static void OnGetRelatedComponents(Component[] components, List<Component> relatedComponents)
        {
            if (components == null) return;

            foreach (Component component in components)
            {
                if (component is Button || component is Toggle || component is InputField)
                {
                    Text[] texts = component.gameObject.GetComponentsInChildren<Text>();
                    if (texts.Length > 0) relatedComponents.AddRange(texts);
                }
                else if (component is Dropdown)
                {
                    Transform labelTransform = component.gameObject.transform.Find("Label");
                    if (labelTransform != null)
                    {
                        Text text = labelTransform.GetComponent<Text>();
                        relatedComponents.Add(text);
                    }
                }
            }
        }
    }
}