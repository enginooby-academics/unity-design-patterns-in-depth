using UnityEngine;
#if ODIN_INSPECTOR

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Audio {
  [RequireComponent(typeof(AudioSource))]
  public class AudioSourceOperator : ComponentOperator<AudioSource> {
    [SerializeField] private SFXDataPreset _sfxDataPreset;

    [SerializeField] private SFXTarget _sfxTarget;

    public AudioSource AudioSource { get; private set; }

    private void Awake() {
      AudioSource = GetComponent<AudioSource>();
    }

    public void Play(SFXAction sfxAction) {
      _sfxDataPreset.PlayRandom(AudioSource, _sfxTarget, sfxAction);
    }
  }
}