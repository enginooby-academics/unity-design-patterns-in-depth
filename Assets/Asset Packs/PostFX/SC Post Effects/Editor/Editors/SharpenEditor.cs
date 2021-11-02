#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEditor.Rendering;

namespace SCPE
{
#if URP
    [VolumeComponentEditor(typeof(Sharpen))]
    sealed class SharpenEditor : VolumeComponentEditor
    {
        SerializedDataParameter amount;
        SerializedDataParameter radius;
        
        private bool isSetup;

        public override void OnEnable()
        {
            base.OnEnable();

            var o = new PropertyFetcher<Sharpen>(serializedObject);
            isSetup = AutoSetup.ValidEffectSetup<SharpenRenderer>();

            amount = Unpack(o.Find(x => x.amount));
            radius = Unpack(o.Find(x => x.radius));
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("sharpen");

            SCPE_GUI.DisplaySetupWarning<SharpenRenderer>(ref isSetup);

            PropertyField(amount);
            PropertyField(radius);
        }
    }
#endif
}