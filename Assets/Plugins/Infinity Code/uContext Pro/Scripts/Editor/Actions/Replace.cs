/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using InfinityCode.uContext;
using InfinityCode.uContext.Actions;
using InfinityCode.uContext.Attributes;
using UnityEngine;

namespace InfinityCode.uContextPro.Actions
{
    [RequireSelected]
    public class Replace : ActionItem
    {
        public override float order
        {
            get { return -905; }
        }

        protected override void Init()
        {
            guiContent = new GUIContent(Icons.replace, "Replace");
        }

        public override void Invoke()
        {
            uContextMenu.Close();
            Tools.Replace.Show(targets);
        }
    }
}