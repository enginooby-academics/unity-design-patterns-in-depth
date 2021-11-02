using UnityEngine;

namespace SCPE
{
    public class SunshaftsBase
    {
        public enum BlendMode
        {
            Additive,
            Screen
        }

        public enum SunShaftsResolution
        {
            High = 1,
            Normal = 2,
            Low = 3,
        }

        public enum Pass
        {
            SkySource,
            RadialBlur,
            Blend
        }

        public static void AddShaftCaster()
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
                Debug.LogError("<b>Sunshafts:</b> No object with the name 'Directional Light' or 'Directional light' could be found");
                return;
            }

            SunshaftCaster caster = directionalLight.GetComponent<SunshaftCaster>();

            if (!caster)
            {
                caster = directionalLight.AddComponent<SunshaftCaster>();
                Debug.Log("\"SunshaftCaster\" component was added to the <b>" + caster.gameObject.name + "</b> GameObject", caster.gameObject);
            }

            if (caster.enabled == false)
            {
                caster.enabled = true;
            }
        }
    }
}