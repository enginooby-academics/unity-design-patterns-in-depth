#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;

namespace TemplateMethodPattern.Case1.Base {
  public abstract class Pickup : MonoBehaviour {
    // TODO: Find and assign FXs for each pickup
    // TODO: Implement size, color pickups
    [HorizontalGroup("FX"), LabelWidth(30)]
    [SerializeField, LabelText("VFX")]
    private ParticleSystem _vfx;

    [HorizontalGroup("FX"), LabelWidth(30)]
    [SerializeField, LabelText("SFX")]
    private AudioClip _sfx;

    /// <summary>
    /// * The 'Template Method'
    /// </summary>
    private void OnTriggerEnter(Collider other) {
      if (other.TryGetComponent<Player>(out var player)) {
        PlayVFX();
        PlaySFX();
        OnPickedUp(player);
        Disappear();
      }
    }

    protected abstract void OnPickedUp(Player player);

    private void PlayVFX() {
    }

    private void PlaySFX() {

    }

    private void Disappear() {
      Destroy(gameObject);
    }
  }
}
