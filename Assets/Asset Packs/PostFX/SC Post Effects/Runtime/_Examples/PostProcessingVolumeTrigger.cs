// SC Post Effects
// Staggart Creations
// http://staggart.xyz

using UnityEngine;
#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
    [ExecuteInEditMode]
    public class PostProcessingVolumeTrigger : MonoBehaviour
    {
#if PPS
        [Header("Target volume")]
        public PostProcessVolume volume;
        [Space]

        public float decreaseSpeed = 1f;

        private float currentWeight = 0f;

        private void OnEnable()
        {
            //No volume assigned, try to get from current object
            if (volume == null)
            {
                volume = this.GetComponent<PostProcessVolume>();
                if (volume) volume.weight = 0f;
            }
            //Set start initial weight to 0
            else
            {
                volume.weight = 0f;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Trigger();
        }

        /// <summary>
        /// Sets the weight of the current volume to 1, after which it will decrease by the set descreaseSpeed value
        /// </summary>
        public void Trigger()
        {
            //Bump up weight to 1
            currentWeight = 1f;
        }

        private void Update()
        {
            //Decreased weight
            currentWeight = Mathf.Clamp01(currentWeight - Time.deltaTime * decreaseSpeed);

            if (!volume) return;

            //Set current weight
            volume.weight = currentWeight;
        }
#else
        [Header("Post Processing package not installed!")]
        public bool error = true;
#endif
    }
}