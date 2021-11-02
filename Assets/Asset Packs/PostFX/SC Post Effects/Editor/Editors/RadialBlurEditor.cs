#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEditor.Rendering;

namespace SCPE
{
#if URP
    [VolumeComponentEditor(typeof(RadialBlur))]
    sealed class RadialBlurEditor : VolumeComponentEditor
    {
        SerializedDataParameter amount;
        SerializedDataParameter iterations;

        private bool isSetup;

        public override void OnEnable()
        {
            base.OnEnable();

            var o = new PropertyFetcher<RadialBlur>(serializedObject);
            isSetup = AutoSetup.ValidEffectSetup<RadialBlurRenderer>();

            amount = Unpack(o.Find(x => x.amount));
            iterations = Unpack(o.Find(x => x.iterations));
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("radial-blur");

            SCPE_GUI.DisplaySetupWarning<RadialBlurRenderer>(ref isSetup);

            PropertyField(amount);
            PropertyField(iterations);
        }
    }
#endif
}