using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Pool;

// ? Group PoolObject w/ SpawnWaveObject
public class PoolObject : MonoBehaviour {
  public IObjectPool<GameObject> pool;

  // [BoxGroup("Release Mode")]
  [LabelText("On Became Invisible")] public bool releaseOnBecameInvisible;

  // [BoxGroup("Release Mode")]
  [Min(0f)] public float lifespan;

  // ? Add Area

  private void Start() {
    ProcessLifespan();
  }

  private void OnEnable() {
    ProcessLifespan();
  }

  private void ProcessLifespan() {
    if (lifespan != 0) StartCoroutine(ReleaseToPoolCoroutine(lifespan));
  }

  private void OnBecameInvisible() {
    if (releaseOnBecameInvisible) ReleaseToPool();
  }

  private IEnumerator ReleaseToPoolCoroutine(float delay) {
    yield return new WaitForSeconds(delay);
    ReleaseToPool();
  }

  public void ReleaseToPool() {
    // print("Release to pool");
    if (!CanReleaseToPool) return;
    pool?.Release(gameObject);
  }

  public bool CanReleaseToPool => gameObject.activeInHierarchy && gameObject && gameObject.activeSelf;

  private void OnDestroy() {
    // IMPL: remove from pool
  }
}
