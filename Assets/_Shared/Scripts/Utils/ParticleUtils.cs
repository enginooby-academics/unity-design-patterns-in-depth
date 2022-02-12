using UnityEngine;

public static class ParticleUtils {
  public static void Play(this ParticleSystem[] particles) {
    foreach (var particle in particles) particle?.Play();
  }
}