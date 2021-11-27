using System.Collections.Generic;
using UnityEngine;

namespace Flyweight.Unity.Prefab {
  public class BoidsController : Flyweight.BoidsController {
    [SerializeField]
    private List<Boid> boidPrefabs = new List<Boid>();

    protected override void SpawnBoids() {
      for (int i = 0; i < boidAmount; i++) {
        Boid newBoid = Instantiate(boidPrefabs.GetRandom(), RandomPosInCage, RandomRot);
        _boids.Add(newBoid);
      }
    }
  }
}
