using UnityEngine;
using Sirenix.OdinInspector;

namespace Enginoobz.Audio {
  [RequireComponent(typeof(AudioSource))]
  public class AudioSourceOperator : ComponentOperator<AudioSource> {
    [SerializeField, InlineEditor]
    private SFXDataPreset _sfxDataPreset;

    [SerializeField]
    private SFXTarget _sfxTarget;

    private AudioSource _audioSource;

    public AudioSource AudioSource => _audioSource;

    public void Play(SFXAction sfxAction) {
      _sfxDataPreset.PlayRandom(_audioSource, _sfxTarget, sfxAction);
    }

    private void Awake() {
      _audioSource = GetComponent<AudioSource>();
    }
  }
}
