using UnityEngine;

public static class AudioUtils {

  /// <summary>
  /// Play by AudioSource from GameManager
  /// </summary>
  public static void PlayOneShot(this AudioClip audioClip) {
    if (!audioClip) return;
    GameManager.Instance.audioSource.PlayOneShot(audioClip);
  }
}