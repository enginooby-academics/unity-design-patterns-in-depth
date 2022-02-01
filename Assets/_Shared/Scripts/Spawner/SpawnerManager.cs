using UnityEngine;
// using QFSW.QC;

// [CommandPrefix("sp.")]
public class SpawnerManager : MonoBehaviour {
  [SerializeField] private Spawner butterflySpawner;
  [SerializeField] private Spawner spellSpawner;

  [Header("[Test spawning]")]
  [SerializeField] private Spawner currentSpawner;
  [SerializeField] private KeyCode spawnPreviousKey;
  [SerializeField] private KeyCode spawnCurrentKey;
  [SerializeField] private KeyCode spawnNextKey;

  private Camera currentCamera;
  private Selector selector;

  void Start() {
    butterflySpawner ??= LoadSpawnerInResources("Butterfly Spawner");
    spellSpawner ??= LoadSpawnerInResources("Spell Spawner");
    currentSpawner ??= spellSpawner;
    selector ??= FindObjectOfType<Selector>();
    currentCamera = Camera.main;
  }

  private Spawner LoadSpawnerInResources(string spawnerName) {
    return Resources.Load<Spawner>($"Spawners/{spawnerName}");
  }

  // [Command("butterfly-at-selection")]
  public void SpawnButterflyAtSelection(int prefabId = -1, bool asChild = false) {
    butterflySpawner?.Spawn(target: selector.CurrentSelectedObject, prefabId: prefabId, asChild: asChild);
  }

  // [Command("butterfly-at-camera")]
  public void SpawnButterflyAtPlayer(int prefabId = -1) {
    butterflySpawner?.Spawn(target: currentCamera.gameObject, prefabId: prefabId, asChild: false);
  }

  // [Command("spell-at-selection")]
  public void SpawnSpellAtSelection(int prefabId = -1, bool asChild = false) {
    spellSpawner?.Spawn(target: selector.CurrentSelectedObject, prefabId: prefabId, asChild: asChild);
  }

  // [Command("spell-at-camera")]
  public void SpawnSpellAtPlayer(int prefabId = -1) {
    spellSpawner?.Spawn(target: currentCamera.gameObject, prefabId: prefabId, asChild: false);
  }

  // Quick test spwaning prefabs
  private void Update() {
    if (spawnCurrentKey.IsUp()) SpawnCurrentAtPlayerOrSelection();
    if (spawnPreviousKey.IsUp()) SpawnPreviousAtPlayerOrSelection();
    if (spawnNextKey.IsUp()) SpawnNextAtPlayerOrSelection();
  }

  private void SpawnCurrentAtPlayerOrSelection() {
    if (selector.CurrentSelectedObject) currentSpawner.SpawnAssetOnPos(selector.CurrentSelectedObject.transform.position);
    else currentSpawner.SpawnAssetOnPos(currentCamera.transform.position);
  }

  private void SpawnPreviousAtPlayerOrSelection() {
    currentSpawner.assetCollection.GetAndSetToPreviousItem();
    SpawnCurrentAtPlayerOrSelection();
  }

  private void SpawnNextAtPlayerOrSelection() {
    currentSpawner.assetCollection.GetAndSetToNextItem();
    SpawnCurrentAtPlayerOrSelection();
  }
}
