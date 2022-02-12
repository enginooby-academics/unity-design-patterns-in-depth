using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

// ? Group PoolObject w/ SpawnWaveObject
public class PoolObject : MonoBehaviour {
  // [BoxGroup("Release Mode")]
  [LabelText("On Became Invisible")] public bool ReleaseOnBecameInvisible;

  // [BoxGroup("Release Mode")]
  [Min(0f)] public float Lifespan;

  public IObjectPool<GameObject> _pool;

  /// <summary>
  ///   Method 1: Specific pool object (vs. this class as uniform pool object) to add additional logic for cleaning item.
  ///   Alternative to onCleanForPoolEvent.
  /// </summary>
  private IPoolObject _specificPoolObject;

  public bool CanReleaseToPool => gameObject.activeInHierarchy && gameObject && gameObject.activeSelf;

  private void Awake() {
    _specificPoolObject = GetComponent<IPoolObject>();
  }

  private void OnEnable() {
    if (Lifespan != 0) ProcessLifespan();
    onReuseEvent?.Invoke();
    _specificPoolObject?.OnPoolReuse();
  }

  private void OnDestroy() {
    // IMPL: remove from pool
  }

  private void OnBecameInvisible() {
    if (ReleaseOnBecameInvisible) ReleaseToPool();
  }

  /// <summary>
  ///   Method 2: Event to add additional logic for cleaning specific pooled object (e.g. re-enable projectile to flying when
  ///   reuse).
  /// </summary>
  public event Action onReuseEvent;

  // ? Add Area

  public void Init(IObjectPool<GameObject> pool) {
    _pool = pool;
    if (Lifespan != 0) ProcessLifespan();
  }

  private void ProcessLifespan() {
    // print("ProcessLifespan");
    StartCoroutine(ReleaseToPoolCoroutine(Lifespan));
  }

  private IEnumerator ReleaseToPoolCoroutine(float delay) {
    yield return new WaitForSeconds(delay);
    ReleaseToPool();
  }

  public void ReleaseToPool() {
    // print("Release to pool");
    if (!CanReleaseToPool) return;
    _pool?.Release(gameObject);
  }
}