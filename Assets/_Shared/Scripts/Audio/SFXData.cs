using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

// TODO: Rename to SFXVariation

namespace Enginoobz.Audio {
  [CreateAssetMenu(fileName = "SFX_", menuName = "Audio/SFX Data", order = 0)]
  /// <summary>
  /// List of audio clips for a certain event (e.g. play taked damage, enemy dead).
  /// </summary>
  public class SFXData : ScriptableObject {
    // TODO: Turn mode: random, random iterate, iterate, random other than last

    [HorizontalGroup("SFX Filter", LabelWidth = 45)] [SerializeField]
    private SFXTarget _target;

    [HorizontalGroup("SFX Filter", LabelWidth = 45)] [SerializeField]
    private SFXAction _action;

    [SerializeField]
    [MinMaxSlider(0, 1, true)]
    [OnValueChanged(nameof(UpdateGlobalVolumeRange))]
    [LabelText("Global Volume")]
    private Vector2 _globalVolumeRange = Vector2.one;

    [SerializeField]
    [MinMaxSlider(-3, 3, true)]
    [OnValueChanged(nameof(UpdateGlobalPitchRange))]
    [LabelText("Global Pitch")]
    private Vector2 _globalPitchRange = Vector2.one;

    [SerializeField] [HideLabel] private List<AudioClipWrapper> _audioClipWrappers = new List<AudioClipWrapper>();

    private AudioClipWrapper _lastClipWrapper;

    private AudioSource _previewAudioSource;

    // [SerializeField]
    // [InlineEditor]
    // [HideInInspector]
    // private List<AudioClip> _clips = new List<AudioClip>();

    // [SerializeField, HideLabel, BoxGroup("Clips")]
    // private AssetCollection<AudioClip> _Sfxs;

    public SFXTarget Target => _target;
    public SFXAction Action => _action;

    private void OnEnable() {
      _previewAudioSource = EditorUtility
        .CreateGameObjectWithHideFlags("Audio Preview", HideFlags.HideAndDontSave,
          typeof(AudioSource)).GetComponent<AudioSource>();
    }

    private void OnDisable() {
      DestroyImmediate(_previewAudioSource);
    }

    private void UpdateGlobalVolumeRange() {
      _audioClipWrappers.ForEach(element => element.VolumeRange = _globalVolumeRange);
    }

    private void UpdateGlobalPitchRange() {
      _audioClipWrappers.ForEach(element => element.PitchRange = _globalPitchRange);
    }

    [Button]
    public void Preview() {
      PlayRandom(_previewAudioSource);
    }

    public void PlayRandom(AudioSource audioSource) {
      _lastClipWrapper = _audioClipWrappers.GetRandomOtherThan(_lastClipWrapper);
      _lastClipWrapper.Play(audioSource);
    }

    public void PlayRandom(AudioSourceOperator audioSourceOperator) {
      PlayRandom(audioSourceOperator.AudioSource);
    }
  }

  public enum SFXTarget {
    Any = 0,
    Player = 1,
    Enemy = 2
  }

  public enum SFXAction {
    Damaged = 0,
    Die = 1,
    Attack = 2
  }
}