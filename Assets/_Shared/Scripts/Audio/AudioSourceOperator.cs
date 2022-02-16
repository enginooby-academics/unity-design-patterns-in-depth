using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

namespace Enginooby.Audio {
  [RequireComponent(typeof(AudioSource))]
  public class AudioSourceOperator : ComponentOperator<AudioSource> {
    [SerializeField] [InlineEditor] private SFXDataPreset _sfxDataPreset;

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