#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEditor.Rendering;

namespace SCPE
{
#if URP
    [VolumeComponentEditor(typeof(BlackBars))]
    sealed class BlackBarsEditor : VolumeComponentEditor
    {
        SerializedDataParameter mode;
        SerializedDataParameter size;
        SerializedDataParameter maxSize;

        private bool isSetup;

        public override void OnEnable()
        {
            base.OnEnable();

            var o = new PropertyFetcher<BlackBars>(serializedObject);
            isSetup = AutoSetup.ValidEffectSetup<BlackBarsRenderer>();

            mode = Unpack(o.Find(x => x.mode));
            size = Unpack(o.Find(x => x.size));
            maxSize = Unpack(o.Find(x => x.maxSize));
        }

        public override string GetDisplayTitle()
        {
            return "Black Bars (" + (BlackBars.Direction)mode.value.enumValueIndex + ")";
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("black-bars");

            SCPE_GUI.DisplaySetupWarning<BlackBarsRenderer>(ref isSetup);

            PropertyField(mode);
            PropertyField(size);
            PropertyField(maxSize);
        }
    }
}
#endif