/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using InfinityCode.uContext;
using InfinityCode.uContext.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.Tools
{
    [InitializeOnLoad]
    public static class SearchScript
    {
        static SearchScript()
        {
            KeyManager.KeyBinding binding = KeyManager.AddBinding();
            binding.OnValidate += OnValidate;
            binding.OnInvoke += OnInvoke;
        }

        private static void OnInvoke()
        {
            Search.OnInvoke();
            Search.instance.searchText = ":script";
            Search.instance.setSelectionIndex = 0;
            Search.searchMode = 2;
        }

        private static bool OnValidate()
        {
            if (!Prefs.searchScript) return false;

            Event e = Event.current;
            return e.modifiers == Prefs.searchScriptModifiers && e.keyCode == Prefs.searchScriptKeyCode;
        }
    }
}