using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

// ? Implement generic
// ? Events
public enum GizmosMode {
  OnSelected,
  Always
}

// [TypeInfoBox("Spawner usages: gun, repeating backgrounds, enemy/obstacle waves")]
/// <summary>
///   Spawner usages: gun, repeating backgrounds, enemy/obstacle waves
/// </summary>
public class Spawner : MonoBehaviourBase {
  #region INSPECTOR FORMATTING ===================================================================================================================================

  private const int SectionSpace = -7;

  #endregion ===================================================================================================================================

  // TODO: integrate pool w/ wave spawning mode
  [SerializeField] [HideLabel] private Pool _pool;

  protected override void Awake() {
    if (_pool.IsEnabled) _pool.Init(currentPrefab);
  }

  private void Reset() {
    if (_pool.IsEnabled) _pool.Init(currentPrefab);
    spawningArea.SetGameObject(gameObject);
  }

  protected override void Start() {
    // Reset();
    _autoSpawnScheduler.Init(this, () => Spawn());
    saveSpawnedObjects = false; // FIX: enable this and Destroy spawned objects by Boundary cause error
    activeCurrentAmount = activeMaxAmount;
    if (enableAdjacentSpawning) previousSpawnedSize = GetPrefabSize(assetCollection.Retrieve());
    if (enableWaveSpawning) InitWaveSpawning();
  }

  protected override void Update() {
    ProcessActiveSpawn();
  }

  private void OnDrawGizmos() {
    if (gizmosMode == GizmosMode.Always) spawningArea.DrawGizmos();
  }

  private void OnDrawGizmosSelected() {
    if (gizmosMode == GizmosMode.OnSelected) spawningArea.DrawGizmos();
  }

  public List<GameObject> Spawn() {
    if (rotationMode == Rotation.Identity) return SpawnAsset();
    if (rotationMode == Rotation.Prefab) return SpawnAssetKeepRotation();
    return Enumerable.Empty<GameObject>().ToList();
  }

  public GameObject Spawn(GameObject target, int prefabId = -1, bool asChild = false) {
    // if (prefabId < -1 || prefabId > prefabs.Count - 1) return null;

    // if (prefabId != -1) currentPrefab = prefabs[prefabId];
    var instance = SpawnAssetOnPos(target.transform.position);
    if (asChild) instance.transform.SetParent(target.transform);
    return instance;
  }

  private GameObject SpawnIndividual(Vector3 pos, bool keepPrefabRotation = false) {
    currentPrefab = assetCollection.Retrieve();
    var rotation = keepPrefabRotation ? currentPrefab.transform.rotation : Quaternion.identity;

    GameObject spawnedObject = null;
    if (_pool.IsEnabled) {
      _pool.Prefab = currentPrefab;
      // spawnedObject = GetInstanceFromPool(pos, keepPrefabRotation);
      spawnedObject = _pool.GetInstance(pos);
    }
    else {
      spawnedObject = InstantiateUtils.Instantiate(pos, currentPrefab, keepPrefabPos: keepPrefabPosition,
        parent: parentObject, rotation: rotation);
    }

    if (saveSpawnedObjects) spawnedObjects.Add(spawnedObject);
    if (moveToLastSpawnedObject) transform.position = spawnedObject.transform.position;
    if (activeMode == ActiveMode.Active && spawnedObject) spawnedObject.SetActive(true);
    if (activeMode == ActiveMode.Inactive && spawnedObject) spawnedObject.SetActive(false);
    // UTIL UnityEditor
    var unityEditorIsPlaying = false;
#if UNITY_EDITOR
    unityEditorIsPlaying = EditorApplication.isPlaying;
#endif
    if (enableLifeTime && unityEditorIsPlaying)
      Destroy(spawnedObject, lifeTimeRange.Random); // ? remove from spawnedObjects
    return spawnedObject;
  }

  private List<GameObject> SpawnIndividuals(bool keepPrefabRotation = false) {
    var newSpawnedObjects = new List<GameObject>();

    if (spawningArea.IsAxixType)
      newSpawnedObjects.Add(SpawnIndividual(spawningArea.areaAxis.Random, keepPrefabRotation));

    if (spawningArea.IsPointType) SpawnLocationPoint(keepPrefabRotation, newSpawnedObjects);

    if (enableAdjacentSpawning) SpawnLocationAdjacent(keepPrefabRotation, newSpawnedObjects);

    return newSpawnedObjects;
  }

  #region CLASS INFO ===================================================================================================================================

  #endregion ===================================================================================================================================

  #region ASSET COLLECTION ===================================================================================================================================

  [HorizontalLine(color: EColor.Red)]
  [PropertySpace(SpaceBefore = SectionSpace + 10)]
  [HideLabel]
  [DisplayAsString(false)]
  [ShowInInspector]
  private const string ObjectSelectionSpace = "Configure resources and selection mode to spawn";

  [Sirenix.OdinInspector.OnValueChanged(nameof(OnAssetCollectionChanged), true)]
  [Sirenix.OdinInspector.InfoBox("If using SceneAsset, pay attention not to destroy the asset blueprint.",
    InfoMessageType.Warning)]
  [Sirenix.OdinInspector.BoxGroup("Asset Collection")]
  [SerializeField]
  [HideLabel]
  public AssetCollection<GameObject> assetCollection = new AssetCollection<GameObject>();

  private GameObject currentPrefab;

  private void OnAssetCollectionChanged() {
    assetCollection.OnItemsChanged();
  }

  #endregion ===================================================================================================================================

  #region UNIVERSAL SPAWNED ATTRIBUTES ===================================================================================================================================

  // TODO: Setup other common component for spawned object such as Projectile, Boundary, Trigger
  // TODO: Scale & Rotation random range, tag, layer
  [PropertySpace(SpaceBefore = SectionSpace)] [HideLabel] [DisplayAsString(false)] [ShowInInspector]
  private const string OBJECT_ATTRIBUTES_SPACE = "";

  [Sirenix.OdinInspector.BoxGroup("Spawned Attributes")]
  [InlineButton(nameof(SetSelfAsParent), "Self")]
  [InlineButton(nameof(SetNoneAsParent), "None")]
  [SerializeField]
  private Transform parentObject;

  private void SetSelfAsParent() {
    parentObject = transform;
  }

  private void SetNoneAsParent() {
    parentObject = null;
  }

  public enum ActiveMode {
    Active,
    Inactive,
    Prefab
  }

  [Sirenix.OdinInspector.BoxGroup("Spawned Attributes")]
  [Tooltip(
    "Spawn new object and set it active/inactive or same as asset. Useful when using SceneAsset and deactivate it but need to spawn active instances.")]
  [EnumToggleButtons]
  [SerializeField]
  private ActiveMode activeMode = ActiveMode.Prefab;

  private enum Rotation {
    Identity,
    Prefab
  }

  // CONSIDER: Separate rotation and selection modes for Auto and Active Spawning
  // CONSIDER: Replace by keepPrefabRotation
  [Sirenix.OdinInspector.BoxGroup("Spawned Attributes")] [EnumToggleButtons] [SerializeField]
  private Rotation rotationMode = Rotation.Identity;

  [Sirenix.OdinInspector.BoxGroup("Spawned Attributes")] [EnumToggleButtons] [SerializeField]
  private AxisFlag keepPrefabPosition = (AxisFlag) 1;

  [Sirenix.OdinInspector.BoxGroup("Spawned Attributes")] [SerializeField] [LabelText("Lifetime")]
  private bool enableLifeTime;

  [Sirenix.OdinInspector.BoxGroup("Spawned Attributes")]
  [Sirenix.OdinInspector.EnableIf(nameof(enableLifeTime))]
  [SerializeField]
  [HideLabel]
  private Vector2Wrapper lifeTimeRange = new Vector2Wrapper(Vector2.zero, 0);

  // ? FXs on Destroy

  #endregion ===================================================================================================================================

  #region SPAWNING LOCATION ===================================================================================================================================

  // TODO: Spawning Target for LocationArea & Location Point (defaut target is the Spawner)
  // TODO: Implement multiple Spawning Location at the same time

  [PropertySpace(SpaceBefore = SectionSpace)] [HideLabel] [DisplayAsString(false)] [ShowInInspector]
  private const string SPAWNING_LOCATION_SPACE = "";

  [Sirenix.OdinInspector.BoxGroup("Spawning Location")]
  [Sirenix.OdinInspector.OnValueChanged(nameof(OnSpawningAreaChange), true)]
  [SerializeField]
  [HideLabel]
  private Area spawningArea = new Area();

  private enum PointSpawnMode {
    Iterate,
    RandomIterate,
    RandomOne,
    RandomAll
  }

  private Transform pointCurrentIterate;

  [Sirenix.OdinInspector.BoxGroup("Spawning Location")]
  [Sirenix.OdinInspector.ShowIf(nameof(IsPointArea))]
  [Sirenix.OdinInspector.OnValueChanged(nameof(OnSpawningAreaChange), true)]
  [SerializeField]
  [EnumToggleButtons]
  [HideLabel]
  private PointSpawnMode pointSpawnMode = PointSpawnMode.RandomAll;

  [Sirenix.OdinInspector.BoxGroup("Spawning Location")]
  [Sirenix.OdinInspector.ShowIf(nameof(IsPointAreaAndRandomAll))]
  [Sirenix.OdinInspector.ProgressBar(0f, 100f)]
  // [Range(0f, 100f)]
  [SerializeField]
  [LabelText("Probability")]
  [SuffixLabel("%")]
  private float pointProbability = 50f;

  private void OnSpawningAreaChange() {
    if (spawningArea.IsPointType) {
      var points = (spawningArea.currentArea as AreaPoint).pointTransforms;
      if (points.IsUnset()) return;
      pointCurrentIterate = points.GetLast();
      if (pointSpawnMode == PointSpawnMode.RandomIterate) points.Shuffle();
    }
  }

  private bool IsPointArea() => spawningArea.IsPointType;

  private bool IsPointAreaAndRandomAll() => spawningArea.IsPointType && pointSpawnMode == PointSpawnMode.RandomAll;

  // REFACTOR
  private void SpawnLocationPoint(bool keepPrefabRotation, List<GameObject> newSpawnedObjects) {
    var points = (spawningArea.CurrentArea as AreaPoint).pointTransforms;

    if (pointSpawnMode == PointSpawnMode.RandomAll)
      points.ForEach(point => {
        if (pointProbability.Percent()) newSpawnedObjects.Add(SpawnIndividual(point.position, keepPrefabRotation));
      });

    if (pointSpawnMode == PointSpawnMode.RandomOne) {
      var postToSpawn = points.GetRandom().position;
      newSpawnedObjects.Add(SpawnIndividual(postToSpawn, keepPrefabRotation));
    }

    if (pointSpawnMode == PointSpawnMode.Iterate) {
      pointCurrentIterate = points.NavNext(pointCurrentIterate);
      newSpawnedObjects.Add(SpawnIndividual(pointCurrentIterate.position, keepPrefabRotation));
    }

    if (pointSpawnMode == PointSpawnMode.RandomIterate) {
      pointCurrentIterate = points.NavNext(pointCurrentIterate);
      newSpawnedObjects.Add(SpawnIndividual(pointCurrentIterate.position, keepPrefabRotation));
    }
  }

  #endregion ===================================================================================================================================

  #region SPAWNING LOCATION ADJACENT ===================================================================================================================================

  [DetailedInfoBox("Click to see Adjacent Location usage...",
    "- Spawn new object next to the last spawned object. Used for endless background objects. \n"
    + "- Attention: colliders on prefabs (to get size), Boundary (destroy action) & Auto spawn rate.")]
  [FoldoutGroup("Spawning Location/Adjacent")]
  [SerializeField]
  private bool enableAdjacentSpawning;

  [FoldoutGroup("Spawning Location/Adjacent")]
  [Sirenix.OdinInspector.InfoBox("Useful for Trigger Spawning")]
  [Sirenix.OdinInspector.EnableIf(nameof(enableAdjacentSpawning))]
  [SerializeField]
  private bool moveSpawnerToMark = true;

  [FoldoutGroup("Spawning Location/Adjacent")]
  [Sirenix.OdinInspector.EnableIf(nameof(enableAdjacentSpawning))]
  [SerializeField]
  private GameObject adjacentMarkPrefab;

  [FoldoutGroup("Spawning Location/Adjacent")]
  [Sirenix.OdinInspector.EnableIf(nameof(enableAdjacentSpawning))]
  [SerializeField]
  private GameObject adjacentMark;

  [FoldoutGroup("Spawning Location/Adjacent")]
  [Sirenix.OdinInspector.EnableIf(nameof(enableAdjacentSpawning))]
  [SerializeField]
  private float adjacentOffset;

  [FoldoutGroup("Spawning Location/Adjacent")]
  [Sirenix.OdinInspector.EnableIf(nameof(enableAdjacentSpawning))]
  [ShowInInspector]
  [DisplayAsString(false)]
  private float previousSpawnedSize;

  // UTIL
  private float GetPrefabSize(GameObject prefab, Axis axis = Axis.X) {
    var boxCollider = currentPrefab.GetComponent<BoxCollider>();
    var size = 0f;
    if (axis == Axis.X) size = boxCollider.size.x * prefab.transform.localScale.x;
    // print("Spawned prefab size: " + size);
    return size + adjacentOffset;
  }

  private void SpawnLocationAdjacent(bool keepPrefabRotation, List<GameObject> newSpawnedObjects) {
    var posToSpawn = transform.position;
    previousSpawnedSize = GetPrefabSize(currentPrefab);
    // AxisBitmask persistentPrefabPositionAxis = keepPrefabPosition;
    if (adjacentMark != null) {
      posToSpawn = adjacentMark.transform.position + new Vector3(previousSpawnedSize / 2, 0, 0);
      keepPrefabPosition = (AxisFlag) 1;
    }

    var spawnedObject = SpawnIndividual(posToSpawn, keepPrefabRotation);
    var pos = spawnedObject.transform.position;
    newSpawnedObjects.Add(spawnedObject);
    var endpointPos = new Vector3(pos.x + previousSpawnedSize / 2, pos.y, pos.z);
    var endpoint = new GameObject("Adjacent Mark");
    if (adjacentMarkPrefab == null) // endpoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      endpoint.transform.position = endpointPos;
    else
      endpoint = Instantiate(adjacentMarkPrefab, endpointPos, Quaternion.identity);

    endpoint.transform.SetParent(spawnedObject.transform);
    adjacentMark = endpoint;

    if (moveSpawnerToMark) transform.position = adjacentMark.transform.position;
  }

  #endregion ===================================================================================================================================

  // TODO: Remove. This point feature is already included in spawningArea

  #region SPAWNING LOCATION POINT ===================================================================================================================================

  private readonly List<GameObject> spawnPoints = new List<GameObject>();

  private void GetSpawnPoints() {
    foreach (Transform child in transform)
      if (child.gameObject.GetComponent<SpawnPoint>() != null)
        spawnPoints.Add(child.gameObject);
  }

  // UTIL generics
  private GameObject CreateSpawnPoint(Vector3 pos, string name = "Spawn Point") {
    var spawnPoint = new GameObject(name);
    spawnPoint.AddComponent<SpawnPoint>();
    spawnPoint.transform.position = pos;
    spawnPoint.transform.SetParent(transform);
    return spawnPoint;
  }

  #endregion ===================================================================================================================================


  #region AUTO SPAWNING - spawning by intervals ===================================================================================================================================

  [SerializeField] [HideLabel] private IntervalActionScheduler _autoSpawnScheduler;

  public IntervalActionScheduler AutoSpawnScheduler => _autoSpawnScheduler;

  #endregion ===================================================================================================================================

  #region ACTIVE SPAWNING - spawning by player ===================================================================================================================================

  // [PropertySpace(SpaceBefore = SECTION_SPACE)]
  // [HideLabel, DisplayAsString(false), ShowInInspector]
  // const string ACTIVE_SPAWNING_SPACE = "";

  [TabGroup("Spawning Mode", "Active Spawning")] [SerializeField]
  private KeyCode activeSpawnKey;

  [TabGroup("Spawning Mode", "Active Spawning")] [Sirenix.OdinInspector.MinValue(0)] [SerializeField]
  private float activeRate = 5f;

  private float nextTimeToActiveSpawn;

  // IMPL loading: player can spawn prefab continously util maxPrefab, then wait for reload time
  [TabGroup("Spawning Mode", "Active Spawning")] [Sirenix.OdinInspector.MinValue(1)] [SerializeField]
  private int activeMaxAmount = 10;

  private int activeCurrentAmount; // the amount that player can spawn

  [TabGroup("Spawning Mode", "Active Spawning")]
  [SuffixLabel("(seconds)")]
  [Sirenix.OdinInspector.MinValue(0f)]
  [SerializeField]
  private float activeReloadTime = 1f;

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

  [TabGroup("Spawning Mode", "Trigger Spawning")] [SerializeField]
  private bool enableTriggerSpawning;

  // TODO: find target for triggering by tag, name, layer, type
  // event trigger: on enter/exit
  // helper function to setup collider in Edit Mode
  // ? Setup EventManager component

  private void OnTriggerEnter(Collider other) {
    if (!enableTriggerSpawning) return;
    if (other.CompareTag("Player")) Spawn();
  }

  #endregion ===================================================================================================================================

  #region WAVE SPAWNING - next wave is spawned when current wave is destroyed ===================================================================================================================================

  // Spawn amount: Increment / Custom
  // Spawn time distance: sim / n each time
  [TabGroup("Spawning Mode", "Wave Spawning")] [SerializeField]
  private bool enableWaveSpawning;

  // Increment mode
  [TabGroup("Spawning Mode", "Wave Spawning")] [SerializeField] [Min(0)] [LabelText("Delay first wave")]
  private int waveSpawningDelayFirst;

  [TabGroup("Spawning Mode", "Wave Spawning")] [SerializeField] [Min(0)] [LabelText("Delay between waves")]
  private int waveSpawningDelay;

  [TabGroup("Spawning Mode", "Wave Spawning")] [SerializeField] [Min(0)] [LabelText("Delay between objects")]
  private int waveObjectSpawningDelay;

  [TabGroup("Spawning Mode", "Wave Spawning")] [SerializeField] [Min(1)]
  private int firstWaveAmount;

  [TabGroup("Spawning Mode", "Wave Spawning")] [SerializeField] [Sirenix.OdinInspector.MinMaxSlider(0, 20, true)]
  private Vector2Int amountIncrementRange;

  private List<GameObject> currentWaveSpawnedObjects;
  private int currentWaveAmount;
  private int currentWaveCount;

  private void InitWaveSpawning() {
    Invoke(nameof(SpawnFirstWave), waveSpawningDelayFirst);
  }

  private void SpawnFirstWave() {
    currentWaveAmount = firstWaveAmount;
    currentWaveCount = firstWaveAmount;
    for (var i = 0; i < currentWaveAmount; i++) Invoke(nameof(SpawnWaveObject), waveObjectSpawningDelay * i);
  }

  public void SpawnNewWave() {
    currentWaveAmount += amountIncrementRange.Random();
    currentWaveCount = currentWaveAmount;
    for (var i = 0; i < currentWaveAmount; i++) Invoke(nameof(SpawnWaveObject), waveObjectSpawningDelay * i);
  }

  private void SpawnWaveObject() {
    var instance = Spawn()[0];
    var tracker = instance.AddComponent<SpawnWaveTracker>();
    tracker.spawner = this;
  }

  public void OnWaveSpawnedDestroy() {
    currentWaveCount--;
    if (currentWaveCount <= 0) Invoke(nameof(SpawnNewWave), waveSpawningDelay);
  }

  #endregion


  #region SPAWNER CONFIG ===================================================================================================================================

  [Sirenix.OdinInspector.BoxGroup("Spawner Config")] [SerializeField] [LabelText("Spawn Amount/Time")]
  private Vector2Wrapper spawnAmountRangePerTime = new Vector2Wrapper(new Vector2(1, 1), 0);

  [Sirenix.OdinInspector.BoxGroup("Spawner Config")] [SerializeField] [EnumToggleButtons]
  private GizmosMode gizmosMode = GizmosMode.Always;

  [Sirenix.OdinInspector.BoxGroup("Spawner Config")]
  [Sirenix.OdinInspector.InfoBox("Useful for Trigger Spawning")]
  [ToggleLeft]
  [SerializeField]
  private bool moveToLastSpawnedObject; // TODO: add offset attribute

  [Sirenix.OdinInspector.BoxGroup("Spawner Config")]
  [InlineButton(nameof(DestroySpawnedObjects), "Destroy All")]
  [ToggleLeft]
  [SerializeField]
  private bool saveSpawnedObjects;

  private void DestroySpawnedObjects() {
    spawnedObjects.ForEach(DestroyImmediate);
    spawnedObjects.Clear();
  }

  [Sirenix.OdinInspector.BoxGroup("Spawner Config")]
  [Sirenix.OdinInspector.ShowIf(nameof(saveSpawnedObjects))]
  [ShowInInspector]
  private List<GameObject> spawnedObjects; // TODO: make list non-editable from Inspector

  #endregion ===================================================================================================================================

  #region TEST SPAWNING ===================================================================================================================================

  [PropertySpace(SpaceBefore = SectionSpace)] [HideLabel] [DisplayAsString(false)] [ShowInInspector]
  private const string TEST_SPACE = "";

  // TODO: Create custom composite attribute

  public GameObject SpawnAssetOnPos(Vector3 dest) {
    var spawnedObject = Instantiate(assetCollection.Retrieve(), dest, Quaternion.identity);
    if (saveSpawnedObjects) spawnedObjects.Add(spawnedObject);
    return spawnedObject;
  }

  [Sirenix.OdinInspector.BoxGroup("Test")]
  [Sirenix.OdinInspector.Button(ButtonSizes.Medium)]
  [PropertyTooltip("Button Tooltip")]
  [GUIColor(.6f, 1f, .6f)]
  public List<GameObject> SpawnAsset() {
    var spawnedObjs = new List<GameObject>();
    for (var i = 0; i < spawnAmountRangePerTime.RandomInt; i++) spawnedObjs.AddRange(SpawnIndividuals());
    return spawnedObjs;
  }

  // ! Keep rotation useful for projectiles
  [Sirenix.OdinInspector.BoxGroup("Test")]
  [Sirenix.OdinInspector.Button(ButtonSizes.Medium)]
  [GUIColor(.6f, 1f, .6f)]
  public List<GameObject> SpawnAssetKeepRotation() {
    var spawnedObjs = new List<GameObject>();
    for (var i = 0; i < spawnAmountRangePerTime.RandomInt; i++) spawnedObjs.AddRange(SpawnIndividuals(true));
    return spawnedObjs;
  }

  #endregion ===================================================================================================================================

  #region SETUP HELPERS - functions to setup common use cases: gun, endless background, obstacle

  #endregion ===================================================================================================================================
}