using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool; // TODO: Add Unity 2022 version directive

// TODO: Generics - GameObject...
[Serializable, InlineProperty]
public class Pool {
  // [PropertySpace(SpaceBefore = SECTION_SPACE)]
  [HideLabel, DisplayAsString(false), ShowInInspector]
  const string POOL_CONFIG_SPACE = "";

  // [ToggleGroup(nameof(enablePool), groupTitle: "Pool Config")]
  [FoldoutGroup("Pool Config")]
  [OnValueChanged(nameof(OnPoolEnabled))]
  [SerializeField]
  private bool _enabled = true;

  public bool IsEnabled => _enabled;

  private void OnPoolEnabled() {
    // if (enablePool) enableLifeTime = false;
  }

  // [ToggleGroup(nameof(enablePool))]
  [FoldoutGroup("Pool Config")]
  [SerializeField, LabelText("Collision Check")]
  private bool _collisionCheck = true;

  // [ToggleGroup(nameof(enablePool))]
  [FoldoutGroup("Pool Config")]
  [SerializeField, LabelText("Release On Invisible")]
  private bool _releaseOnBecameInvisible;

  // [ToggleGroup(nameof(enablePool))]
  [FoldoutGroup("Pool Config")]
  [Tooltip("Value 0 means disable lifespan.")]
  [SerializeField, Min(0), LabelText("Release By Lifespan")]
  private float _releaseByLifespan;

  // [ToggleGroup(nameof(enablePool))]
  [FoldoutGroup("Pool Config")]
  [SerializeField, Min(1), LabelText("Max Size")]
  private int _maxSize = 20;

  // [ToggleGroup(nameof(enablePool))]
  [FoldoutGroup("Pool Config")]
  [SerializeField, Min(1), LabelText("Default Capacity")]
  private int _defaultCapacity = 1000;

  [HideInInspector]
  public GameObject Prefab;

  private IObjectPool<GameObject> _pool;

  public void Init(GameObject prefab) { // ? How about randomize mode
    Prefab = prefab;
    _pool = new ObjectPool<GameObject>(
      CreateInstance,
      OnPoolGet,
      OnPoolRelease,
      OnPoolDestroy,
      maxSize: _maxSize,
      defaultCapacity: _defaultCapacity
    );
  }

  private GameObject CreateInstance() {
    // Debug.Log("CreateInstance from pool");
    GameObject instance = UnityEngine.Object.Instantiate(Prefab);
    // float poolObjectLifespan = poolObjectReleaseByLifespan;

    if (instance.TryGetComponent<PoolObject>(out var poolObject)) {
      // respect pre-setup params from the PoolObject component
    } else {
      poolObject = instance.AddComponent<PoolObject>();
      poolObject.Lifespan = _releaseByLifespan;
      poolObject.ReleaseOnBecameInvisible = _releaseOnBecameInvisible;
    }

    poolObject.Init(_pool);

    return instance;
  }

  // ! Get() from Pool does not respect Retrieve Mode of Collection
  // private GameObject GetInstanceFromPool(Vector3 pos, bool keepPrefabRotation) {
  //   if (pool == null) InitPool();
  //   GameObject instance = pool.Get();
  //   instance.transform.position = pos;
  //   // instance.transform.rotation = rot;
  //   instance.transform.UpdatePosOnAxis(target: instance.transform, axis: keepPrefabPosition);
  //   if (parentObject && instance) instance.transform.SetParent(parentObject);

  //   return instance;
  // }

  public GameObject GetInstance(Vector3 pos) {
    if (_pool == null) Init(Prefab);

    GameObject instance = _pool.Get();
    instance.transform.position = pos;
    return instance;
  }

  private bool CanObjectReleasedToPool(GameObject poolObject) {
    return poolObject.activeInHierarchy && poolObject;
  }

  private void OnPoolRelease(GameObject poolObject) {
    if (!CanObjectReleasedToPool(poolObject)) return;

    poolObject.SetActive(false);
  }

  private IEnumerator ReleaseToPoolCoroutine(GameObject poolObject, float delay) {
    yield return new WaitForSeconds(delay);
    OnPoolRelease(poolObject);
  }

  private void OnPoolGet(GameObject poolObject) {
    poolObject.SetActive(true);
  }

  private void OnPoolDestroy(GameObject poolObject) {
    // DestroyImmediate(poolObject); // TODO: for Edit Mode
    UnityEngine.Object.Destroy(poolObject);
  }
}