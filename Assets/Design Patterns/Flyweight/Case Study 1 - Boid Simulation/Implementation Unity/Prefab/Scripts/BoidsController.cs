using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

namespace Flyweight.Unity.Prefab {
  public class BoidsController : Flyweight.BoidsController {
    [SerializeField] [InlineEditor] private List<Flyweight.Boid> boidPrefabs = new List<Flyweight.Boid>();

    protected override void SpawnBoids() {
      for (var i = 0; i < boidAmount; i++) {
        var newBoid = Instantiate(boidPrefabs.GetRandom(), RandomPosInCage, RandomRot);
        _boids.Add(newBoid);
      }
    }
  }
}