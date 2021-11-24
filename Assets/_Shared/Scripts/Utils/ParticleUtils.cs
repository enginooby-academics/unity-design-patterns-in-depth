using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ParticleUtils {
  public static void Play(this ParticleSystem[] particles) {
    foreach (var particle in particles) {
      particle?.Play();
    }
  }
}