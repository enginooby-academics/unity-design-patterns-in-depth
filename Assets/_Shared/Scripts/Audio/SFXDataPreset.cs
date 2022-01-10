using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// TODO: Rename to SFXCollection/SFXTheme

namespace Enginoobz.Audio {
  [CreateAssetMenu(fileName = "SFXPreset_", menuName = "Audio/SFX Data Preset", order = 0)]
  /// <summary>
  /// Centralization for all SFXData.
  /// </summary>
  public class SFXDataPreset : ScriptableObject {
    [SerializeField, InlineEditor]
    private List<SFXData> _sfxDatas = new List<SFXData>();

    public void PlayRandom(AudioSource audioSource, SFXTarget sfxTarget, SFXAction sfxAction) {
      SFXData sfxData = _sfxDatas.Find(sfxData => sfxData.Target == sfxTarget && sfxData.Action == sfxAction)
                      ?? _sfxDatas.Find(sfxData => sfxData.Target == SFXTarget.Any && sfxData.Action == sfxAction)
                      ?? _sfxDatas.Find(sfxData => sfxData.Action == sfxAction);
      if (!sfxData) {
        Debug.LogError("No SFXData found for " + Enum.GetName(typeof(SFXTarget), sfxTarget) + " " + Enum.GetName(typeof(SFXAction), sfxAction));
      } else {
        sfxData.PlayRandom(audioSource);
      }
    }
  }
}
