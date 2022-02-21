using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// TODO: Extend SO
// + Rename to AudioClipData

namespace Enginooby.Audio {
  [Serializable]
  [InlineProperty]
  public class AudioClipWrapper {
    [FoldoutGroup("$GROUP_NAME")] [SerializeField] [InlineButton(nameof(PlayInEditMode), "▶")] [HideLabel]
    private AudioClip _audioClip;

    [FoldoutGroup("$GROUP_NAME")] [SerializeField] [MinMaxSlider(0, 1, true)]
    private Vector2 _volumeRange = Vector2.one;

    [FoldoutGroup("$GROUP_NAME")] [SerializeField] [MinMaxSlider(0, 3, true)]
    private Vector2 _pitchRange = Vector2.one;

    private readonly string GROUP_NAME = "Audio Clip";

    public Vector2 VolumeRange {
      get => _volumeRange;
      set => _volumeRange = value; // TODO: validation
    }

    public Vector2 PitchRange {
      get => _pitchRange;
      set => _pitchRange = value; // TODO: validation
    }

    // TODO: play with customized volume and pitch in Edit Mode
    public void PlayInEditMode() {
      if (_audioClip) PlayClip(_audioClip);
    }

    public void Play(AudioSource audioSource) {
      audioSource.volume = _volumeRange.Random();
      audioSource.pitch = _pitchRange.Random();
      audioSource.clip = _audioClip;
      audioSource.Play();
    }

    // UTIL
    public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false) {
      var unityEditorAssembly = typeof(AudioImporter).Assembly;

      var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
      var method = audioUtilClass.GetMethod(
        "PlayPreviewClip",
        BindingFlags.Static | BindingFlags.Public,
        null,
        new[] {typeof(AudioClip), typeof(int), typeof(bool)},
        null
      );

      Debug.Log(method);
      method.Invoke(
        null,
        new object[] {clip, startSample, loop}
      );
    }
  }
}