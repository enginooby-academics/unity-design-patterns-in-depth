using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Pool;

// ? Group PoolObject w/ SpawnWaveObject
public class PoolObject : MonoBehaviour {
  public IObjectPool<GameObject> pool;

  // [BoxGroup("Release Mode")]
  [LabelText("On Became Invisible")]
  public bool releaseOnBecameInvisible;

  // [BoxGroup("Release Mode")]
  [Min(0f)]
  public float lifespan;

  /// <summary>
  /// Method 1: Specific pool object (vs. this class as uniform pool object) to add additional logic for cleaning item. Alternative to onCleanForPoolEvent.
  /// </summary>
  private IPoolObject _specificPoolObject;

  /// <summary>
  /// Method 2: Event to add additional logic for cleaning specific pooled object (e.g. re-enable projectile to flying when reuse).
  /// </summary>
  public event System.Action onReuseEvent;

  // ? Add Area

  private void Start() {
    ProcessLifespan();
    _specificPoolObject = GetComponent<IPoolObject>();
  }

  private void OnEnable() {
    ProcessLifespan();
    onReuseEvent?.Invoke();
    _specificPoolObject?.OnPoolReuse();
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
