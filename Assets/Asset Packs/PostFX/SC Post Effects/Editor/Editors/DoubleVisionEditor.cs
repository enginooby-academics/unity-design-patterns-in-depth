#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEditor.Rendering;

namespace SCPE
{
#if URP
    [VolumeComponentEditor(typeof(DoubleVision))]
    sealed class DoubleVisionEditor : VolumeComponentEditor
    {
        SerializedDataParameter mode;
        SerializedDataParameter intensity;

        private bool isSetup;

        public override void OnEnable()
        {
            base.OnEnable();

            var o = new PropertyFetcher<DoubleVision>(serializedObject);
            isSetup = AutoSetup.ValidEffectSetup<DoubleVisionRenderer>();

            mode = Unpack(o.Find(x => x.mode));
            intensity = Unpack(o.Find(x => x.intensity));
        }


        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("double-vision");

            SCPE_GUI.DisplaySetupWarning<DoubleVisionRenderer>(ref isSetup);

            PropertyField(mode);
            PropertyField(intensity);
        }
    }
#endif
}
