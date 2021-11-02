#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEditor.Rendering;

namespace SCPE
{
#if URP
    [VolumeComponentEditor(typeof(Colorize))]
    sealed class ColorizeEditor : VolumeComponentEditor
    {
        SerializedDataParameter mode;
        SerializedDataParameter intensity;
        SerializedDataParameter colorRamp;

        private bool isSetup;

        public override void OnEnable()
        {
            base.OnEnable();

            var o = new PropertyFetcher<Colorize>(serializedObject);
            isSetup = AutoSetup.ValidEffectSetup<ColorizeRenderer>();

            mode = Unpack(o.Find(x => x.mode));
            intensity = Unpack(o.Find(x => x.intensity));
            colorRamp = Unpack(o.Find(x => x.colorRamp));
        }

        public override string GetDisplayTitle()
        {
            return "Colorize (" + mode.value.enumDisplayNames[mode.value.intValue] + ")";
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("colorize");

            SCPE_GUI.DisplaySetupWarning<ColorizeRenderer>(ref isSetup);

            PropertyField(mode);
            PropertyField(intensity);
            PropertyField(colorRamp);

            if (colorRamp.value.objectReferenceValue)
            {
                SCPE.CheckGradientImportSettings(colorRamp);
            }
        }
    }
#endif
}
