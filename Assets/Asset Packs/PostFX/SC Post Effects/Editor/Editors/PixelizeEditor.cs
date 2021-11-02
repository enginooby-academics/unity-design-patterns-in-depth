#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEditor.Rendering;

namespace SCPE
{
#if URP
    [VolumeComponentEditor(typeof(Pixelize))]
    sealed class PixelizeEditor : VolumeComponentEditor
    {
        SerializedDataParameter amount;
        
        private bool isSetup;

        public override void OnEnable()
        {
            base.OnEnable();

            var o = new PropertyFetcher<Pixelize>(serializedObject);
            isSetup = AutoSetup.ValidEffectSetup<PixelizeRenderer>();

            amount = Unpack(o.Find(x => x.amount));
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("pixelize");

            SCPE_GUI.DisplaySetupWarning<PixelizeRenderer>(ref isSetup);

            PropertyField(amount);
        }
    }
#endif
}