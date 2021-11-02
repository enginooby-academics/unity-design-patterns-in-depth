/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using InfinityCode.uContext;
using InfinityCode.uContext.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro
{
    [InitializeOnLoad]
    public static class uContextProMenu
    {
        static uContextProMenu()
        {
            Prefs.OnInsertProMenuItems += OnInsertProMenuItems;
            Welcome.OnCheckUpdates += OnWelcomeCheckUpdates;
            Welcome.OnInitLate += OnInitLate;
        }

        private static void OnInitLate()
        {
            Welcome.headerStyle.normal.background = ResourcesPRO.Load<Texture2D>("Textures/Welcome/Logo PRO.png");
            Welcome.headerStyle.padding = new RectOffset(270, 0, 30, 0);
        }

        private static void OnWelcomeCheckUpdates()
        {
            if (Welcome.DrawButton(Welcome.updateTexture, "Check Updates", "Perhaps a new version is already waiting for you. Check it!"))
            {
                Windows.Updater.OpenWindow();
            }
        }

        private static void OnInsertProMenuItems(GenericMenuEx menu)
        {
            menu.Add("Check Updates", Windows.Updater.OpenWindow);
        }
    }
}