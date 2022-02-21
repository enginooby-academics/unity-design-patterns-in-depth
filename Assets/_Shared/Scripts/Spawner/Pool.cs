using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_2021_1_OR_NEWER
using UnityEngine.Pool;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// TODO: Generics - GameObject...
/// <summary>
///   Default pool: disable object on release, enable object on retrieve, destroy object on destroy
/// </summary>
[Serializable]
[InlineProperty]
public class Pool {
  // [PropertySpace(SpaceBefore = SECTION_SPACE)]
  [HideLabel] [DisplayAsString(false)] [ShowInInspector]
  private const string POOL_CONFIG_SPACE = "";

  // [ToggleGroup(nameof(enablePool), groupTitle: "Pool Config")]
  [FoldoutGroup("Pool Config")] [OnValueChanged(nameof(OnPoolEnabled))] [SerializeField]
  private bool _enabled = true;

  // [ToggleGroup(nameof(enablePool))]
  [FoldoutGroup("Pool Config")] [SerializeField] [LabelText("Collision Check")]
  private bool _collisionCheck = true;

  // [ToggleGroup(nameof(enablePool))]
  [FoldoutGroup("Pool Config")] [SerializeField] [LabelText("Release On Invisible")]
  private bool _releaseOnBecameInvisible;

  // [ToggleGroup(nameof(enablePool))]
  [FoldoutGroup("Pool Config")]
  [Tooltip("Value 0 means disable lifespan.")]
  [SerializeField]
  [Min(0)]
  [LabelText("Release By Lifespan")]
  private float _releaseByLifespan;

  // [ToggleGroup(nameof(enablePool))]
  [FoldoutGroup("Pool Config")] [SerializeField] [Min(1)] [LabelText("Max Size")]
  private int _maxSize = 20;

  // [ToggleGroup(nameof(enablePool))]
  [FoldoutGroup("Pool Config")] [SerializeField] [Min(1)] [LabelText("Default Capacity")]
  private int _defaultCapacity = 1000;

  [HideInInspector] public GameObject Prefab;

#if UNITY_2021_1_OR_NEWER
  private IObjectPool<GameObject> _pool;
#endif
  public bool IsEnabled => _enabled;

  private void OnPoolEnabled() {
    // if (enablePool) enableLifeTime = false;
  }

  // ! Get() from Pool does not respect Retrieve Mode of Collection
  public GameObject GetInstance(Vector3 pos) {
    GameObject instance = default;
#if UNITY_2021_1_OR_NEWER
    if (_pool == null) Init(Prefab);

    instance = _pool.Get();
#else
     instance = CreateInstance();
#endif
    instance.transform.position = pos;
    return instance;
  }

  public void Init(GameObject prefab) {
    // ? How about randomize mode
    Prefab = prefab;
#if UNITY_2021_1_OR_NEWER
    _pool = new ObjectPool<GameObject>(
      CreateInstance,
      OnPoolGet,
      OnPoolRelease,
      OnPoolDestroy,
      maxSize: _maxSize,
      defaultCapacity: _defaultCapacity
    );
#endif
  }

  private GameObject CreateInstance() {
    // Debug.Log("CreateInstance from pool");
    var instance = Object.Instantiate(Prefab);
    // float poolObjectLifespan = poolObjectReleaseByLifespan;

    if (instance.TryGetComponent<PoolObject>(out var poolObject)) {
      // respect pre-setup params from the PoolObject component
    }
    else {
      poolObject = instance.AddComponent<PoolObject>();
      poolObject.Lifespan = _releaseByLifespan;
      poolObject.ReleaseOnBecameInvisible = _releaseOnBecameInvisible;
    }

#if UNITY_2021_1_OR_NEWER
    poolObject.Init(_pool);
#endif

    return instance;
  }

  private bool IsObjectReadyToRelease(GameObject poolObject) => poolObject.activeInHierarchy && poolObject;

  private void OnPoolRelease(GameObject poolObject) {
    if (!IsObjectReadyToRelease(poolObject)) return;

    poolObject.SetActive(false);
  }

  private IEnumerator ReleaseToPoolCoroutine(GameObject poolObject, float delay) {
    yield return new WaitForSeconds(delay);
    OnPoolRelease(poolObject);
  }

  private void OnPoolGet(GameObject poolObject) => poolObject.SetActive(true);

  // DestroyImmediate(poolObject); // TODO: for Edit Mode
  private void OnPoolDestroy(GameObject poolObject) => Object.Destroy(poolObject);
}