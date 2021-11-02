/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using InfinityCode.uContext;
using InfinityCode.uContext.Actions;
using InfinityCode.uContextPro.Windows;
using UnityEngine;

namespace InfinityCode.uContextPro.Actions
{
    public class UpdateAvailable : ActionItem, IValidatableLayoutItem
    {
        public override float order
        {
            get { return 950; }
        }

        protected override void Init()
        {
            guiContent = new GUIContent(Icons.updateAvailable, "Update Available");
        }

        public override void Invoke()
        {
            Updater.OpenWindow();
        }

        public bool Validate()
        {
            Updater.CheckNewVersionAvailable();
            return Updater.hasNewVersion;
        }
    }
}