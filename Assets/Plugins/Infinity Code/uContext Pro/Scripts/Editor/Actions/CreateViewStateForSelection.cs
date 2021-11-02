/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using InfinityCode.uContext;
using InfinityCode.uContext.Actions;
using InfinityCode.uContextPro.Tools;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.uContextPro.Actions
{
    [InitializeOnLoad]
    public static class CreateViewStateForSelection
    {
        static CreateViewStateForSelection()
        {
            SceneViewActions.OnViewStateCreateFromSelection += InsertMenuItem;
        }

        private static void InsertMenuItem(GenericMenuEx menu)
        {
            if (Selection.gameObjects.Length != 1) return;
            GameObject go = Selection.activeGameObject;
            if (go.scene.name == null) return;

            menu.Add("View States/Create For Selection", SelectionViewStates.AddToSelection);
        }
    }
}