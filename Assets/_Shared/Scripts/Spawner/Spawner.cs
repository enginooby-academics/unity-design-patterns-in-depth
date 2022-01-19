using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;

// ? Implement generic
// ? Events
public enum GizmosMode { OnSelected, Always }

// [TypeInfoBox("Spawner usages: gun, repeating backgrounds, enemy/obstacle waves")]
/// <summary>
/// Spawner usages: gun, repeating backgrounds, enemy/obstacle waves
/// </summary>
public class Spawner : MonoBehaviourBase {
  // TODO: integrate pool w/ wave spawning mode
  [SerializeField, HideLabel]
  private Pool _pool;

  private void Awake() {
    if (_pool.IsEnabled) _pool.Init(currentPrefab);
  }

  private void Reset() {
    if (_pool.IsEnabled) _pool.Init(currentPrefab);
    spawningArea.SetGameObject(gameObject);
  }

  private void Start() {
    // Reset();
    _autoSpawnScheduler.Init(actionOwner: this, action: () => Spawn());
    saveSpawnedObjects = false; // FIX: enable this and Destroy spawned objects by Boundary cause error
    activeCurrentAmount = activeMaxAmount;
    if (enableAdjacentSpawning) previousSpawnedSize = GetPrefabSize(assetCollection.Retrieve());
    if (enableWaveSpawning) InitWaveSpawning();
  }

  private void Update() {
    ProcessActiveSpawn();
  }

  void OnDrawGizmosSelected() {
    if (gizmosMode == GizmosMode.OnSelected) spawningArea.DrawGizmos();
  }

  private void OnDrawGizmos() {
    if (gizmosMode == GizmosMode.Always) spawningArea.DrawGizmos();
  }

  public List<GameObject> Spawn() {
    if (rotationMode == Rotation.Identity) return SpawnAsset();
    if (rotationMode == Rotation.Prefab) return SpawnAssetKeepRotation();
    return Enumerable.Empty<GameObject>().ToList();
  }

  public GameObject Spawn(GameObject target, int prefabId = -1, bool asChild = false) {
    // if (prefabId < -1 || prefabId > prefabs.Count - 1) return null;

    // if (prefabId != -1) currentPrefab = prefabs[prefabId];
    GameObject instance = SpawnAssetOnPos(target.transform.position);
    if (asChild) instance.transform.SetParent(target.transform);
    return instance;
  }

  private GameObject SpawnIndividual(Vector3 pos, bool keepPrefabRotation = false) {
    currentPrefab = assetCollection.Retrieve();
    Quaternion rotation = (keepPrefabRotation) ? currentPrefab.transform.rotation : Quaternion.identity;

    GameObject spawnedObject = null;
    if (_pool.IsEnabled) {
      _pool.Prefab = currentPrefab;
      // spawnedObject = GetInstanceFromPool(pos, keepPrefabRotation);
      spawnedObject = _pool.GetInstance(pos);
    } else {
      spawnedObject = InstantiateUtils.Instantiate(pos: pos, prefab: currentPrefab, keepPrefabPos: keepPrefabPosition, parent: parentObject, rotation: rotation);
    }

    if (saveSpawnedObjects) spawnedObjects.Add(spawnedObject);
    if (moveToLastSpawnedObject) transform.position = spawnedObject.transform.position;
    if (activeMode == ActiveMode.Active && spawnedObject) spawnedObject.SetActive(true);
    if (activeMode == ActiveMode.Inactive && spawnedObject) spawnedObject.SetActive(false);
    // UTIL UnityEditor
    bool unityEditorIsPlaying = false;
#if UNITY_EDITOR
    unityEditorIsPlaying = UnityEditor.EditorApplication.isPlaying;
#endif
    if (enableLifeTime && unityEditorIsPlaying) Destroy(spawnedObject, lifeTimeRange.Random); // ? remove from spawnedObjects
    return spawnedObject;
  }

  private List<GameObject> SpawnIndividuals(bool keepPrefabRotation = false) {
    List<GameObject> newSpawnedObjects = new List<GameObject>();

    if (spawningArea.IsAxixType) {
      newSpawnedObjects.Add(SpawnIndividual(spawningArea.areaAxis.Random, keepPrefabRotation: keepPrefabRotation));
    }

    if (spawningArea.IsPointType) {
      SpawnLocationPoint(keepPrefabRotation, newSpawnedObjects);
    }

    if (enableAdjacentSpawning) {
      SpawnLocationAdjacent(keepPrefabRotation, newSpawnedObjects);
    }

    return newSpawnedObjects;
  }

  #region INSPECTOR FORMATTING ===================================================================================================================================
  const int SECTION_SPACE = -7;
  #endregion ===================================================================================================================================

  #region CLASS INFO ===================================================================================================================================
  #endregion ===================================================================================================================================

  #region ASSET COLLECTION ===================================================================================================================================
  [NaughtyAttributes.HorizontalLine(color: NaughtyAttributes.EColor.Red)]
  [PropertySpace(SpaceBefore = SECTION_SPACE + 10)]
  [HideLabel, DisplayAsString(false), ShowInInspector]
  const string OBJECT_SELECTION_SPACE = "Configure resources and selection mode to spawn";

  [OnValueChanged(nameof(OnAssetCollectionChanged), true)]
  [InfoBox("If using SceneAsset, pay attention not to destroy the asset blueprint.", InfoMessageType.Warning)]
  [BoxGroup("Asset Collection")]
  [SerializeField, HideLabel] public AssetCollection<GameObject> assetCollection = new AssetCollection<GameObject>();
  private GameObject currentPrefab;

  private void OnAssetCollectionChanged() {
    assetCollection.OnItemsChanged();
  }
  #endregion ===================================================================================================================================

  #region UNIVERSAL SPAWNED ATTRIBUTES ===================================================================================================================================
  // TODO: Setup other common component for spawned object such as Projectile, Boundary, Trigger
  // TODO: Scale & Rotation random range, tag, layer
  [PropertySpace(SpaceBefore = SECTION_SPACE)]
  [HideLabel, DisplayAsString(false), ShowInInspector]
  const string OBJECT_ATTRIBUTES_SPACE = "";

  [BoxGroup("Spawned Attributes")]
  [InlineButton(nameof(SetSelfAsParent), "Self")]
  [InlineButton(nameof(SetNoneAsParent), "None")]
  [SerializeField] private Transform parentObject;
  private void SetSelfAsParent() {
    parentObject = transform;
  }
  private void SetNoneAsParent() {
    parentObject = null;
  }

  public enum ActiveMode { Active, Inactive, Prefab }
  [BoxGroup("Spawned Attributes")]
  [Tooltip("Spawn new object and set it active/inactive or same as asset. Useful when using SceneAsset and deactivate it but need to spawn active instances.")]
  [EnumToggleButtons] [SerializeField] private ActiveMode activeMode = ActiveMode.Prefab;

  private enum Rotation { Identity, Prefab }
  // CONSIDER: Separate rotation and selection modes for Auto and Active Spawning
  // CONSIDER: Replace by keepPrefabRotation
  [BoxGroup("Spawned Attributes")]
  [EnumToggleButtons] [SerializeField] private Rotation rotationMode = Rotation.Identity;

  [BoxGroup("Spawned Attributes")]
  [EnumToggleButtons] [SerializeField] private AxisFlag keepPrefabPosition = (AxisFlag)1;

  [BoxGroup("Spawned Attributes")]
  [SerializeField, LabelText("Lifetime")] bool enableLifeTime;

  [BoxGroup("Spawned Attributes")]
  [EnableIf(nameof(enableLifeTime))]
  [SerializeField, HideLabel] Vector2Wrapper lifeTimeRange = new Vector2Wrapper(Vector2.zero, min: 0);
  // ? FXs on Destroy
  #endregion ===================================================================================================================================

  #region SPAWNING LOCATION ===================================================================================================================================
  // TODO: Spawning Target for LocationArea & Location Point (defaut target is the Spawner)
  // TODO: Implement multiple Spawning Location at the same time

  [PropertySpace(SpaceBefore = SECTION_SPACE)]
  [HideLabel, DisplayAsString(false), ShowInInspector]
  const string SPAWNING_LOCATION_SPACE = "";

  [BoxGroup("Spawning Location")]
  [OnValueChanged(nameof(OnSpawningAreaChange), true)]
  [SerializeField, HideLabel] Area spawningArea = new Area();

  enum PointSpawnMode { Iterate, RandomIterate, RandomOne, RandomAll }

  private Transform pointCurrentIterate;

  [BoxGroup("Spawning Location")]
  [ShowIf(nameof(IsPointArea))]
  [OnValueChanged(nameof(OnSpawningAreaChange), true)]
  [SerializeField, EnumToggleButtons, HideLabel] PointSpawnMode pointSpawnMode = PointSpawnMode.RandomAll;

  [BoxGroup("Spawning Location")]
  [ShowIf(nameof(IsPointAreaAndRandomAll))]
  [ProgressBar(0f, 100f)]
  // [Range(0f, 100f)]
  [SerializeField, LabelText("Probability"), SuffixLabel("%")] float pointProbability = 50f;

  private void OnSpawningAreaChange() {
    if (spawningArea.IsPointType) {
      List<Transform> points = (spawningArea.currentArea as AreaPoint).pointTransforms;
      if (points.IsUnset()) return;
      pointCurrentIterate = points.GetLast();
      if (pointSpawnMode == PointSpawnMode.RandomIterate) {
        points.Shuffle();
      }
    }
  }

  private bool IsPointArea() { return spawningArea.IsPointType; }
  private bool IsPointAreaAndRandomAll() { return spawningArea.IsPointType && pointSpawnMode == PointSpawnMode.RandomAll; }

  // REFACTOR
  private void SpawnLocationPoint(bool keepPrefabRotation, List<GameObject> newSpawnedObjects) {
    List<Transform> points = (spawningArea.CurrentArea as AreaPoint).pointTransforms;

    if (pointSpawnMode == PointSpawnMode.RandomAll) {
      points.ForEach(point => {
        if (pointProbability.Percent()) {
          newSpawnedObjects.Add(SpawnIndividual(point.position, keepPrefabRotation: keepPrefabRotation));
        }
      });
    }

    if (pointSpawnMode == PointSpawnMode.RandomOne) {
      Vector3 postToSpawn = points.GetRandom().position;
      newSpawnedObjects.Add(SpawnIndividual(postToSpawn, keepPrefabRotation: keepPrefabRotation));
    }

    if (pointSpawnMode == PointSpawnMode.Iterate) {
      pointCurrentIterate = points.NavNext(pointCurrentIterate);
      newSpawnedObjects.Add(SpawnIndividual(pointCurrentIterate.position, keepPrefabRotation: keepPrefabRotation));
    }

    if (pointSpawnMode == PointSpawnMode.RandomIterate) {
      pointCurrentIterate = points.NavNext(pointCurrentIterate);
      newSpawnedObjects.Add(SpawnIndividual(pointCurrentIterate.position, keepPrefabRotation: keepPrefabRotation));
    }
  }
  #endregion ===================================================================================================================================

  #region SPAWNING LOCATION ADJACENT ===================================================================================================================================
  [DetailedInfoBox("Click to see Adjacent Location usage...",
  "- Spawn new object next to the last spawned object. Used for endless background objects. \n"
  + "- Attention: colliders on prefabs (to get size), Boundary (destroy action) & Auto spawn rate.")]
  [FoldoutGroup("Spawning Location/Adjacent")]
  [SerializeField] bool enableAdjacentSpawning;

  [FoldoutGroup("Spawning Location/Adjacent")]
  [InfoBox("Useful for Trigger Spawning")]
  [EnableIf(nameof(enableAdjacentSpawning))]
  [SerializeField] bool moveSpawnerToMark = true;

  [FoldoutGroup("Spawning Location/Adjacent")]
  [EnableIf(nameof(enableAdjacentSpawning))]
  [SerializeField] private GameObject adjacentMarkPrefab;

  [FoldoutGroup("Spawning Location/Adjacent")]
  [EnableIf(nameof(enableAdjacentSpawning))]
  [SerializeField] private GameObject adjacentMark;

  [FoldoutGroup("Spawning Location/Adjacent")]
  [EnableIf(nameof(enableAdjacentSpawning))]
  [SerializeField] private float adjacentOffset = 0f;

  [FoldoutGroup("Spawning Location/Adjacent")]
  [EnableIf(nameof(enableAdjacentSpawning))]
  [ShowInInspector, DisplayAsString(false)] private float previousSpawnedSize;

  // UTIL
  private float GetPrefabSize(GameObject prefab, Axis axis = Axis.X) {
    BoxCollider boxCollider = currentPrefab.GetComponent<BoxCollider>();
    float size = 0f;
    if (axis == Axis.X) {
      size = boxCollider.size.x * prefab.transform.localScale.x;
    }
    // print("Spawned prefab size: " + size);
    return size + adjacentOffset;
  }

  private void SpawnLocationAdjacent(bool keepPrefabRotation, List<GameObject> newSpawnedObjects) {
    Vector3 posToSpawn = transform.position;
    previousSpawnedSize = GetPrefabSize(currentPrefab);
    // AxisBitmask persistentPrefabPositionAxis = keepPrefabPosition;
    if (adjacentMark != null) {
      posToSpawn = adjacentMark.transform.position + new Vector3(previousSpawnedSize / 2, 0, 0);
      keepPrefabPosition = (AxisFlag)1;
    }
    GameObject spawnedObject = SpawnIndividual(posToSpawn, keepPrefabRotation: keepPrefabRotation);
    Vector3 pos = spawnedObject.transform.position;
    newSpawnedObjects.Add(spawnedObject);
    Vector3 endpointPos = new Vector3(pos.x + previousSpawnedSize / 2, pos.y, pos.z);
    GameObject endpoint = new GameObject("Adjacent Mark");
    if (adjacentMarkPrefab == null) {
      // endpoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      endpoint.transform.position = endpointPos;
    } else {
      endpoint = Instantiate(adjacentMarkPrefab, endpointPos, Quaternion.identity);
    }

    endpoint.transform.SetParent(spawnedObject.transform);
    adjacentMark = endpoint;

    if (moveSpawnerToMark) transform.position = adjacentMark.transform.position;
  }
  #endregion ===================================================================================================================================

  // TODO: Remove. This point feature is already included in spawningArea
  #region SPAWNING LOCATION POINT ===================================================================================================================================
  private List<GameObject> spawnPoints = new List<GameObject>();

  private void GetSpawnPoints() {
    foreach (Transform child in transform) {
      if (child.gameObject.GetComponent<SpawnPoint>() != null) spawnPoints.Add(child.gameObject);
    }
  }

  // UTIL generics
  private GameObject CreateSpawnPoint(Vector3 pos, string name = "Spawn Point") {
    GameObject spawnPoint = new GameObject(name);
    spawnPoint.AddComponent<SpawnPoint>();
    spawnPoint.transform.position = pos;
    spawnPoint.transform.SetParent(transform);
    return spawnPoint;
  }
  #endregion ===================================================================================================================================


  #region AUTO SPAWNING - spawning by intervals ===================================================================================================================================
  [SerializeField, HideLabel]
  private IntervalActionScheduler _autoSpawnScheduler;

  public IntervalActionScheduler AutoSpawnScheduler => _autoSpawnScheduler;
  #endregion ===================================================================================================================================

  #region ACTIVE SPAWNING - spawning by player ===================================================================================================================================
  // [PropertySpace(SpaceBefore = SECTION_SPACE)]
  // [HideLabel, DisplayAsString(false), ShowInInspector]
  // const string ACTIVE_SPAWNING_SPACE = "";

  [TabGroup("Spawning Mode", "Active Spawning")]
  [SerializeField] private KeyCode activeSpawnKey;

  [TabGroup("Spawning Mode", "Active Spawning")]
  [MinValue(0)]
  [SerializeField] private float activeRate = 5f;
  private float nextTimeToActiveSpawn = 0f;

  // IMPL loading: player can spawn prefab continously util maxPrefab, then wait for reload time
  [TabGroup("Spawning Mode", "Active Spawning")]
  [MinValue(1)]
  [SerializeField] private int activeMaxAmount = 10;
  private int activeCurrentAmount; // the amount that player can spawn

  [TabGroup("Spawning Mode", "Active Spawning")]
  [SuffixLabel("(seconds)")]
  [MinValue(0f)]
  [SerializeField] private float activeReloadTime = 1f;

  private void ProcessActiveSpawn() {
    if (activeSpawnKey.IsUp() && activeCurrentAmount > 0 && Time.time >= nextTimeToActiveSpawn) ActiveSpawn();
  }

  private void ActiveSpawn() {
    Spawn();
    nextTimeToActiveSpawn = Time.time + 1f / activeRate;
    activeCurrentAmount--;
  }
  #endregion ===================================================================================================================================

  #region TRIGGER SPAWNING - spawning by events (e.g player reach to the end of last spawned object)
  // [PropertySpace(SpaceBefore = SECTION_SPACE)]
  // [HideLabel, DisplayAsString(false), ShowInInspector]
  // const string TRIGGER_SPAWNING_SPACE = "";

  [TabGroup("Spawning Mode", "Trigger Spawning")]
  [SerializeField] bool enableTriggerSpawning = false;

  // TODO: find target for triggering by tag, name, layer, type
  // event trigger: on enter/exit
  // helper function to setup collider in Edit Mode
  // ? Setup EventManager component

  private void OnTriggerEnter(Collider other) {
    if (!enableTriggerSpawning) return;
    if (other.CompareTag("Player")) {
      Spawn();
    }
  }
  #endregion ===================================================================================================================================

  #region WAVE SPAWNING - next wave is spawned when current wave is destroyed ===================================================================================================================================
  // Spawn amount: Increment / Custom
  // Spawn time distance: sim / n each time
  [TabGroup("Spawning Mode", "Wave Spawning")]
  [SerializeField] bool enableWaveSpawning;

  // Increment mode
  [TabGroup("Spawning Mode", "Wave Spawning")]
  [SerializeField, Min(0), LabelText("Delay first wave")]
  int waveSpawningDelayFirst;
  [TabGroup("Spawning Mode", "Wave Spawning")]
  [SerializeField, Min(0), LabelText("Delay between waves")]
  int waveSpawningDelay;
  [TabGroup("Spawning Mode", "Wave Spawning")]
  [SerializeField, Min(0), LabelText("Delay between objects")]
  int waveObjectSpawningDelay;
  [TabGroup("Spawning Mode", "Wave Spawning")]
  [SerializeField, Min(1)]
  int firstWaveAmount;
  [TabGroup("Spawning Mode", "Wave Spawning")]
  [SerializeField, MinMaxSlider(0, 20, true)]
  Vector2Int amountIncrementRange;

  private List<GameObject> currentWaveSpawnedObjects;
  private int currentWaveAmount;
  private int currentWaveCount;

  private void InitWaveSpawning() {
    Invoke(nameof(SpawnFirstWave), waveSpawningDelayFirst);
  }

  private void SpawnFirstWave() {
    currentWaveAmount = firstWaveAmount;
    currentWaveCount = firstWaveAmount;
    for (int i = 0; i < currentWaveAmount; i++) {
      Invoke(nameof(SpawnWaveObject), waveObjectSpawningDelay * i);
    }
  }

  public void SpawnNewWave() {
    currentWaveAmount += amountIncrementRange.Random();
    currentWaveCount = currentWaveAmount;
    for (int i = 0; i < currentWaveAmount; i++) {
      Invoke(nameof(SpawnWaveObject), waveObjectSpawningDelay * i);
    }
  }

  private void SpawnWaveObject() {
    GameObject instance = Spawn()[0];
    SpawnWaveTracker tracker = instance.AddComponent<SpawnWaveTracker>();
    tracker.spawner = this;
  }

  public void OnWaveSpawnedDestroy() {
    currentWaveCount--;
    if (currentWaveCount <= 0) Invoke(nameof(SpawnNewWave), waveSpawningDelay);
  }
  #endregion


  #region SPAWNER CONFIG ===================================================================================================================================
  [BoxGroup("Spawner Config")]
  [SerializeField, LabelText("Spawn Amount/Time")] Vector2Wrapper spawnAmountRangePerTime = new Vector2Wrapper(new Vector2(1, 1), min: 0, max: 10);

  [BoxGroup("Spawner Config")]
  [SerializeField, EnumToggleButtons] GizmosMode gizmosMode = GizmosMode.Always;

  [BoxGroup("Spawner Config")]
  [InfoBox("Useful for Trigger Spawning")]
  [ToggleLeft]
  [SerializeField] bool moveToLastSpawnedObject = false; // TODO: add offset attribute

  [BoxGroup("Spawner Config")]
  [InlineButton(nameof(DestroySpawnedObjects), "Destroy All")]
  [ToggleLeft]
  [SerializeField] bool saveSpawnedObjects = false;
  private void DestroySpawnedObjects() {
    spawnedObjects.ForEach(DestroyImmediate);
    spawnedObjects.Clear();
  }

  [BoxGroup("Spawner Config")]
  [ShowIf(nameof(saveSpawnedObjects))]
  [ShowInInspector] List<GameObject> spawnedObjects; // TODO: make list non-editable from Inspector
  #endregion ===================================================================================================================================

  #region TEST SPAWNING ===================================================================================================================================
  [PropertySpace(SpaceBefore = SECTION_SPACE)]
  [HideLabel, DisplayAsString(false), ShowInInspector]
  const string TEST_SPACE = "";

  // TODO: Create custom composite attribute

  public GameObject SpawnAssetOnPos(Vector3 dest) {
    GameObject spawnedObject = Instantiate(assetCollection.Retrieve(), dest, Quaternion.identity);
    if (saveSpawnedObjects) spawnedObjects.Add(spawnedObject);
    return spawnedObject;
  }

  [BoxGroup("Test")]
  [Button(ButtonSizes.Medium), PropertyTooltip("Button Tooltip")]
  [GUIColor(.6f, 1f, .6f)]
  public List<GameObject> SpawnAsset() {
    List<GameObject> spawnedObjs = new List<GameObject>();
    for (int i = 0; i < spawnAmountRangePerTime.RandomInt; i++) {
      spawnedObjs.AddRange(SpawnIndividuals());
    }
    return spawnedObjs;
  }

  // ! Keep rotation useful for projectiles
  [BoxGroup("Test")]
  [Button(ButtonSizes.Medium)]
  [GUIColor(.6f, 1f, .6f)]
  public List<GameObject> SpawnAssetKeepRotation() {
    List<GameObject> spawnedObjs = new List<GameObject>();
    for (int i = 0; i < spawnAmountRangePerTime.RandomInt; i++) {
      spawnedObjs.AddRange(SpawnIndividuals(keepPrefabRotation: true));
    }
    return spawnedObjs;
  }

  #endregion ===================================================================================================================================

  #region SETUP HELPERS - functions to setup common use cases: gun, endless background, obstacle
  #endregion ===================================================================================================================================
}
