using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SCPE
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Light))]
    public class FogLightSource : MonoBehaviour
    {
        public Light sunLight;

        //Light component
        public static Vector3 sunDirection;
        public static Color color;
        public static float intensity;

        private void OnEnable()
        {
            sunDirection = -transform.forward;

            if (!sunLight)
            {
                sunLight = this.GetComponent<Light>();
                if (sunLight)
                {
                    color = sunLight.color;
                    intensity = sunLight.intensity;
                }
            }
        }

        private void OnDisable()
        {
            sunDirection = Vector3.zero;
			#if PPS
            Fog.LightDirection = Vector3.zero;
			#endif
        }

        void Update()
        {
            sunDirection = -transform.forward;
            #if PPS
			Fog.LightDirection = sunDirection;
            #endif
            if (sunLight)
            {
                color = sunLight.color;
                intensity = sunLight.intensity;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(FogLightSource))]
    public class FogLightSourceInspector : Editor
    {
        private new SerializedObject serializedObject;
        private SerializedProperty sunLight;

        void OnEnable()
        {
            serializedObject = new SerializedObject(target);

            sunLight = serializedObject.FindProperty("sunLight");
        }

        override public void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(target, "Component");

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(sunLight, new GUIContent("Light"));

            EditorGUILayout.HelpBox("This object is used as the source Directional Light for the Fog effect", MessageType.None);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        public static void AddLightSource()
        {
            GameObject directionalLight = null;

            if (GameObject.Find("Directional Light"))
            {
                directionalLight = GameObject.Find("Directional Light");
            }

            if (!directionalLight)
            {
                if (GameObject.Find("Directional light"))
                {
                    directionalLight = GameObject.Find("Directional light");
                }
            }

            if (!directionalLight)
            {
                Debug.LogError("<b>Fog:</b> No object with the name 'Directional Light' or 'Directional light' could be found");
                return;
            }

            FogLightSource caster = directionalLight.GetComponent<FogLightSource>();

            if (!caster)
            {
                caster = directionalLight.AddComponent<FogLightSource>();
                Debug.Log("\"FogLightSource\" component was added to the <b>" + caster.gameObject.name + "</b> GameObject", caster.gameObject);
            }

            if (caster.enabled == false)
            {
                caster.enabled = true;
            }
        }
    }
#endif
}