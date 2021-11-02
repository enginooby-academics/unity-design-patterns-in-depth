/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using InfinityCode.uContext;
using InfinityCode.uContext.FloatToolbar;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace InfinityCode.uContextPro.Tools
{
    [InitializeOnLoad]
    public static class OpenBestComponent
    {
        static OpenBestComponent()
        {
            ObjectToolbar.OnGenerateLayoutComplete += OnGenerateLayout;
        }

        private static void OnGenerateLayout(List<ObjectToolbar.Item> items)
        {
            if (!Prefs.objectToolbarOpenBestComponent || items.Count == 0 || ObjectToolbar.isMinimized) return;

            Component target = items[0].target as Component;
            if (target == null)
            {
                ObjectToolbar.ShowItem(0);
                return;
            }

            int bestIndex = 0;

            if (items.Count > 1)
            {
                bestIndex = 1;

                Component second = items[1].target as Component;
                if (second != null && (second is CanvasRenderer || second is MeshFilter))
                {
                    bestIndex++;

                    if (items.Count > 3)
                    {
                        Component third = items[2].target as Component;
                        if (third != null && third is Image) bestIndex++;
                    }
                }
            }

            if (bestIndex >= items.Count) bestIndex = 0;

            ObjectToolbar.ShowItem(bestIndex);
        }
    }
}