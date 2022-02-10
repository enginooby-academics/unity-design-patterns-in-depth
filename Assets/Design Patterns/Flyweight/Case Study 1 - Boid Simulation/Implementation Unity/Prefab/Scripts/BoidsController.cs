using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Flyweight.Unity.Prefab {
  public class BoidsController : Flyweight.BoidsController {
    [SerializeField, InlineEditor]
    private List<Flyweight.Boid> boidPrefabs = new List<Flyweight.Boid>();

    protected override void SpawnBoids() {
      for (int i = 0; i < boidAmount; i++) {
        Flyweight.Boid newBoid = Instantiate(boidPrefabs.GetRandom(), RandomPosInCage, RandomRot);
        _boids.Add(newBoid);
      }
    }
  }
}
