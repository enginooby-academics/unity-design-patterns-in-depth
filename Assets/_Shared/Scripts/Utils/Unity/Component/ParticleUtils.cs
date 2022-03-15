using System.Collections.Generic;
using UnityEngine;

public static class ParticleUtils {
  public static void Play(this IEnumerable<ParticleSystem> particles) {
    foreach (var particle in particles) particle.Play();
  }

  public static ParticleSystem WithColor(this ParticleSystem vfx, Color color) {
    vfx.GetComponent<Renderer>().material.color = color;
    return vfx;
  }

  // ? Remove lazy method
  public static ParticleSystem WithColorOf(this ParticleSystem vfx, Material material) => vfx.WithColor(material.color);

  public static ParticleSystem WithColorOf(this ParticleSystem vfx, MeshRenderer meshRenderer) =>
    vfx.WithColor(meshRenderer.material.color);
}