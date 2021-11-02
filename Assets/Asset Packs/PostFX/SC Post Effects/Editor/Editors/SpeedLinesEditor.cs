#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEditor.Rendering;

namespace SCPE
{
#if URP
    [VolumeComponentEditor(typeof(SpeedLines))]
    sealed class SpeedLinesEditor : VolumeComponentEditor
    {
        SerializedDataParameter intensity;
        SerializedDataParameter size;
        SerializedDataParameter falloff;
        SerializedDataParameter noiseTex;

        private bool isSetup;

        public override void OnEnable()
        {
            base.OnEnable();

            var o = new PropertyFetcher<SpeedLines>(serializedObject);
            isSetup = AutoSetup.ValidEffectSetup<SpeedLinesRenderer>();

            intensity = Unpack(o.Find(x => x.intensity));
            size = Unpack(o.Find(x => x.size));
            falloff = Unpack(o.Find(x => x.falloff));
            noiseTex = Unpack(o.Find(x => x.noiseTex));
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("speed-lines");

            SCPE_GUI.DisplaySetupWarning<SpeedLinesRenderer>(ref isSetup);

            PropertyField(noiseTex);
            PropertyField(intensity);
            PropertyField(size);
            PropertyField(falloff);
        }
    }
#endif
}