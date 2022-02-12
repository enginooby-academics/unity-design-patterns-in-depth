using System.Collections.Generic;
using UnityEngine;

namespace Flyweight {
  public abstract class BoidsController : MonoBehaviourSingleton<BoidsController> {
    [Header("BOID PARAMETERS")] [SerializeField]
    protected int boidAmount = 100;

    public float boidSpeed = 10;
    public float boidPerceptionRadius = 10;

    [Header("FORCE PARAMETERS")] public float separationWeight = 1;

    public float cohesionWeight = 1;
    public float alignmentWeight = 1;

    [Header("CAGE PARAMETERS")] public float cageSize = 100;

    public float avoidWallsWeight = 10;
    public float avoidWallsTurnDist = 10;

    protected List<Boid> _boids = new List<Boid>();

    public List<Boid> Boids => _boids;

    // protected virtual void SpawnBoids() {
    //   for (int i = 0; i < boidAmount; i++) {
    //     Boid newBoid = default;

    //     switch (mode) {
    //       // case MODE.None:
    //       //   GameObject newBoidObject = MeshGenerator.Create(MeshGenerator.SHAPE.Cube, randomPos, randomRot, 1.5f);
    //       //   newBoid = newBoidObject.AddComponent(typeof(Boid)) as Boid;

    //       //   switch (randomIndex) {
    //       //     case 0:
    //       //       ((Boid)newBoid).SetSharedData("Flamingo", ENDANGERED_STATUS.VU);
    //       //       break;
    //       //     case 1:
    //       //       ((Boid)newBoid).SetSharedData("Shorebird", ENDANGERED_STATUS.EN);
    //       //       break;
    //       //     case 2:
    //       //       ((Boid)newBoid).SetSharedData("Starling", ENDANGERED_STATUS.CR);
    //       //       break;
    //       //   }
    //       //   break;
    //       case MODE.Prefab:
    //         newBoid = Instantiate(boidPrefabs.GetRandom(), RandomPosInCage, RandomRot);
    //         break;
    //       case MODE.Scriptable_Prefab:
    //         // newBoid = Instantiate(boidScriptablePrefabs[randomIndex], randomPos, randomRot).GetComponent<BoidBase>();
    //         break;
    //     }
    //     _boids.Add(newBoid);
    //   }
    // }


    // HELPER
    public Vector3 RandomPosInCage => new Vector3(Random.Range(-cageSize / 2f, cageSize / 2f),
      Random.Range(-cageSize / 2f, cageSize / 2f), Random.Range(-cageSize / 2f, cageSize / 2f));

    // HELPER
    public Quaternion RandomRot =>
      Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

    private void Awake() {
      // _boids?.Clear();
      SpawnBoids();
    }

    private void OnDrawGizmos() {
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(Vector3.zero, new Vector3(cageSize, cageSize, cageSize));
    }

    protected abstract void SpawnBoids();
  }
}