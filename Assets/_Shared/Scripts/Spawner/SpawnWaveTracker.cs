using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaveTracker : MonoBehaviour {
  public Spawner spawner;

  private void OnDestroy() {
    if (!this.gameObject.scene.isLoaded) return;
    spawner.OnWaveSpawnedDestroy();
  }
}
