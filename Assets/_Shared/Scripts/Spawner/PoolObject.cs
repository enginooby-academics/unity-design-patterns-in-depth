using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Pool;

// ! Do not setup this component beforehand on GameObject (add component on it in Editor)
// ! otherwise pool can not be set. Setup when Instantiate by Pool instead
// ? Group PoolObject w/ SpawnWaveObject
public class PoolObject : MonoBehaviour {
  public IObjectPool<GameObject> pool;

  [BoxGroup("Release Mode")]
  [LabelText("On Became Invisible")] public bool releaseOnBecameInvisible;

  [BoxGroup("Release Mode")]
  [Min(0f)] public float lifespan;

  // ? Add Area

  private void OnBecameInvisible() {
    if (releaseOnBecameInvisible) ReleaseToPool();
  }

  void Start() {
    if (lifespan != 0) Invoke(nameof(ReleaseToPool), lifespan);
  }

  public void ReleaseToPool() {
    pool?.Release(gameObject);
  }
}
