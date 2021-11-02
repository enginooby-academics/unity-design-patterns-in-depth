#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEditor.Rendering;
using UnityEditor;

namespace SCPE
{
#if URP
    [VolumeComponentEditor(typeof(Danger))]
    sealed class DangerEditor : VolumeComponentEditor
    {
        SerializedDataParameter overlayTex;
        SerializedDataParameter color;
        SerializedDataParameter size;
        SerializedDataParameter intensity;

        private bool isSetup;

        public override void OnEnable()
        {
            base.OnEnable();

            var o = new PropertyFetcher<Danger>(serializedObject);
            isSetup = AutoSetup.ValidEffectSetup<DangerRenderer>();

            overlayTex = Unpack(o.Find(x => x.overlayTex));
            color = Unpack(o.Find(x => x.color));
            intensity = Unpack(o.Find(x => x.intensity));
            size = Unpack(o.Find(x => x.size));
        }


        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("danger");

            SCPE_GUI.DisplaySetupWarning<DangerRenderer>(ref isSetup);

            PropertyField(overlayTex);

            if (overlayTex.overrideState.boolValue && overlayTex.value.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Assign a texture", MessageType.Info);
            }

            PropertyField(color);
            PropertyField(intensity);
            PropertyField(size);
        }
    }
#endif
}
