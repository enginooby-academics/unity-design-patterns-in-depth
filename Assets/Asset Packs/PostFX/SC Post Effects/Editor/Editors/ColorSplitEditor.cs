#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEditor.Rendering;

namespace SCPE
{
#if URP
    [VolumeComponentEditor(typeof(ColorSplit))]
    sealed class ColorSplitEditor : VolumeComponentEditor
    {
        SerializedDataParameter mode;
        SerializedDataParameter offset;

        private bool isSetup;

        public override void OnEnable()
        {
            base.OnEnable();

            var o = new PropertyFetcher<ColorSplit>(serializedObject);
            isSetup = AutoSetup.ValidEffectSetup<ColorSplitRenderer>();

            mode = Unpack(o.Find(x => x.mode));
            offset = Unpack(o.Find(x => x.offset));
        }

        public override string GetDisplayTitle()
        {
            return "Color Split (" + mode.value.enumDisplayNames[mode.value.intValue] + ")";
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("color-split");

            SCPE_GUI.DisplaySetupWarning<ColorSplitRenderer>(ref isSetup);

            PropertyField(mode);
            PropertyField(offset);
            
        }
    }
#endif
}
