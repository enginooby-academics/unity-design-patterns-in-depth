#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SCPE
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Light))]
    public class SunshaftCaster : MonoBehaviour
    {
        [Range(0f, 10000f)]
        public float distance = 10000f;
        [Tooltip("Use this to match the casting position to a skybox sun")]
        public bool infiniteDistance = false;
        [Tooltip("This light will be used to sample the intensity if color")]
        public Light sunLight;

        private Vector3 sunPosition;

        //Light component
        public static Color color;
        public static float intensity;

        private void OnEnable()
        {
            sunPosition = this.transform.position;

            if (!sunLight)
            {
                sunLight = this.GetComponent<Light>();
            }
        }

        private void OnDisable()
        {
            sunPosition = Vector3.zero;
#if URP
            Sunshafts.sunPosition = Vector3.zero;
#endif

        }

        private void OnDrawGizmos()
        {
#if URP
            Gizmos.DrawIcon(Sunshafts.sunPosition, "LensFlare Icon", true);
#endif
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 1, 0, 0.5f);
            Gizmos.DrawRay(transform.position, sunPosition);
        }

        void Update()
        {
            sunPosition = -transform.forward * ((infiniteDistance) ? 1E10f : distance);
#if URP
            Sunshafts.sunPosition = sunPosition;
#endif

            if (sunLight)
            {
                color = sunLight.color;
                intensity = sunLight.intensity;

            }
        }
        }

#if UNITY_EDITOR
    [CustomEditor(typeof(SunshaftCaster))]
    public class SunshaftCasterInspector : Editor
    {

        private new SerializedObject serializedObject;
        private SerializedProperty sunLight;
        private SerializedProperty distance;
        private SerializedProperty infiniteDistance;

        void OnEnable()
        {
            serializedObject = new SerializedObject(target);

            sunLight = serializedObject.FindProperty("sunLight");
            distance = serializedObject.FindProperty("distance");
            infiniteDistance = serializedObject.FindProperty("infiniteDistance");
        }

        override public void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(target, "Component");

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(sunLight, new GUIContent("Light"));
            EditorGUILayout.PropertyField(infiniteDistance, new GUIContent("Infinite distance"));
            if (infiniteDistance.boolValue == false) EditorGUILayout.PropertyField(distance, new GUIContent("Distance"));

            EditorGUILayout.HelpBox("This object is used as the source sunshaft caster for all sunshaft effect instances", MessageType.None);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
#endif
}

