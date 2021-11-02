#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEditor;
using UnityEngine;
using UnityEditor.Rendering;

namespace SCPE
{
#if URP
    public class SunshaftsEditorBase
    {
        public static void DrawCasterWarning()
        {
            if (Sunshafts.sunPosition == Vector3.zero)
            {
                EditorGUILayout.HelpBox("No source Directional Light found!\n\nAdd the \"SunshaftCaster\" script to your main light", MessageType.Warning);

                GUILayout.Space(-32);
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Add", GUILayout.Width(60)))
                    {
                        SunshaftsBase.AddShaftCaster();
                    }
                    GUILayout.Space(8);
                }
                GUILayout.Space(11);
            }
        }
    }

    [VolumeComponentEditor(typeof(Sunshafts))]
    sealed class SunshaftsEditor : VolumeComponentEditor
    {
        Sunshafts effect;

        SerializedDataParameter useCasterColor;
        SerializedDataParameter useCasterIntensity;

        SerializedDataParameter resolution;
        SerializedDataParameter sunThreshold;
        SerializedDataParameter blendMode;
        SerializedDataParameter sunColor;
        SerializedDataParameter sunShaftIntensity;
        SerializedDataParameter falloff;

        SerializedDataParameter length;
        SerializedDataParameter highQuality;

        private bool isSetup;

        public override void OnEnable()
        {
            base.OnEnable();

            effect = (Sunshafts)target;
            var o = new PropertyFetcher<Sunshafts>(serializedObject);

            isSetup = AutoSetup.ValidEffectSetup<SunshaftsRenderer>();

            useCasterColor = Unpack(o.Find(x => x.useCasterColor));
            useCasterIntensity = Unpack(o.Find(x => x.useCasterIntensity));

            resolution = Unpack(o.Find(x => x.resolution));
            sunThreshold = Unpack(o.Find(x => x.sunThreshold));
            blendMode = Unpack(o.Find(x => x.blendMode));
            sunColor = Unpack(o.Find(x => x.sunColor));
            sunShaftIntensity = Unpack(o.Find(x => x.sunShaftIntensity));
            falloff = Unpack(o.Find(x => x.falloff));

            length = Unpack(o.Find(x => x.length));
            highQuality = Unpack(o.Find(x => x.highQuality));
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("edge-detection");

            SCPE_GUI.DisplayDocumentationButton("sunshafts");

            SCPE_GUI.DisplayVRWarning();

            SCPE_GUI.ShowDepthTextureWarning();

            SunshaftsEditorBase.DrawCasterWarning();

            SCPE_GUI.DisplaySetupWarning<SunshaftsRenderer>(ref isSetup, false);

            //SCPE_GUI.DrawHeaderLabel("Quality");
            PropertyField(resolution);
            PropertyField(highQuality, new GUIContent("High quality"));

            EditorGUILayout.Space();

            //SCPE_GUI.DrawHeaderLabel("Use values from caster");
            PropertyField(useCasterColor, new GUIContent("Color"));
            PropertyField(useCasterIntensity, new GUIContent("Intensity"));

            EditorGUILayout.Space();

            //SCPE_GUI.DrawHeaderLabel("Sunshafts");
            PropertyField(blendMode);
            if (blendMode.value.intValue == 1) EditorGUILayout.HelpBox("Screen blend mode currrently not supported", MessageType.Warning);
            PropertyField(sunThreshold);
            PropertyField(falloff);
            PropertyField(length);
            if (useCasterColor.value.boolValue == false) PropertyField(sunColor);
            if (useCasterIntensity.value.boolValue == false) PropertyField(sunShaftIntensity);
        }

    }
#endif
}