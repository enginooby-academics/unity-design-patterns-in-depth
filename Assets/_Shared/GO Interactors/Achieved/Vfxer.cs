#if ASSET_MESH_EFFECTS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using QFSW.QC;

//? Rename to MeshVFXer
public class Vfxer : MonoBehaviour {
  // Add a small delay when changing effect 
  // to prevent destroying the previous effect to also destroy the material of the next effect
  const float TRANSITION_DELAY = 0.2f;

  // TODO: Replace by Collection
  [ValueDropdown(nameof(vfxPrefabs))]
  [SerializeField]
  private PSMeshRendererUpdater currentVfxPrefab;
  [SerializeField]
  private List<PSMeshRendererUpdater> vfxPrefabs;

  [Header("[Specialized VFX]")]
  [ValueDropdown(nameof(burnVfxPrefabs))]
  [SerializeField]
  private PSMeshRendererUpdater defaultBurnVfxPrefab;
  [SerializeField]
  private List<PSMeshRendererUpdater> burnVfxPrefabs;


  private void DestroyAllMeshEffects(GameObject target) {
    var meshUpdaters = target.GetComponentsInChildren<PSMeshRendererUpdater>();
    foreach (PSMeshRendererUpdater meshUpdater in meshUpdaters) {
      meshUpdater.IsActive = false;
      Destroy(meshUpdater.gameObject, TRANSITION_DELAY);
    }
  }

  private IEnumerator AddCurrentMeshEffectEnumerator(GameObject target) {
    DestroyAllMeshEffects(target);
    yield return new WaitForSeconds(TRANSITION_DELAY);
    PSMeshRendererUpdater meshUpdater = Instantiate(currentVfxPrefab, target.transform.position, Quaternion.identity);
    meshUpdater.gameObject.transform.SetParent(target.transform);
    meshUpdater.UpdateMeshEffect(target);
  }

  public void ToggleActiveMeshEffect(GameObject target) {
    var meshUpdater = target.GetComponentInChildren<PSMeshRendererUpdater>();
    if (meshUpdater) meshUpdater.IsActive = !meshUpdater.IsActive;
    else StartCoroutine(AddCurrentMeshEffectEnumerator(target));
  }

  public void AddNextMeshEffect(GameObject target) {
    currentVfxPrefab = vfxPrefabs.NavNext(currentVfxPrefab);
    StartCoroutine(AddCurrentMeshEffectEnumerator(target));
  }

  public void AddPreviousMeshEffect(GameObject target) {
    currentVfxPrefab = vfxPrefabs.NavPrevious(currentVfxPrefab);
    StartCoroutine(AddCurrentMeshEffectEnumerator(target));
  }

  public void AddMeshEffect(GameObject target, int effectId) {
    currentVfxPrefab = vfxPrefabs[effectId];
    StartCoroutine(AddCurrentMeshEffectEnumerator(target));
  }

  public void AddMeshEffect(GameObject target, PSMeshRendererUpdater vfxPrefab) {
    currentVfxPrefab = vfxPrefab;
    StartCoroutine(AddCurrentMeshEffectEnumerator(target));
  }

  public void AddCurrentMeshEffectToMultiple(GameObject[] targets) {
    foreach (var target in targets) {
      StartCoroutine(AddCurrentMeshEffectEnumerator(target));
    }
  }

  public void AddFireMeshEffect(GameObject target, int burnEffectId) {
    if (burnEffectId >= burnVfxPrefabs.Count || burnEffectId < 0) {
      print($"Limit: {burnVfxPrefabs.Count - 1}");
      return;
    }

    AddMeshEffect(target, burnVfxPrefabs[burnEffectId]);
  }

  public void AddFireMeshEffect(GameObject target) {
    AddMeshEffect(target, defaultBurnVfxPrefab);
  }

  [Command(CommandPrefix.MeshVfx + CommandTarget.Tag)]
  public void AddMeshEffectByTag(string tag, int effectId) {
    currentVfxPrefab = vfxPrefabs[effectId];
    // TODO: find tag insensitively
    AddCurrentMeshEffectToMultiple(GameObject.FindGameObjectsWithTag(tag));
  }

  [Command(CommandPrefix.MeshVfx + CommandTarget.Tag)]
  public void AddMeshEffectByTag(string tag) {
    AddCurrentMeshEffectToMultiple(GameObject.FindGameObjectsWithTag(tag));
  }

  // public void ToggleCurrentMeshEffect(GameObject target) {
  //   PSMeshRendererUpdater meshUpdater = GetMeshUpdaterByName(target, currentVfxPrefab.name);
  //   if (meshUpdater) meshUpdater.IsActive = !meshUpdater.IsActive;
  //   else StartCoroutine(AddCurrentMeshEffectEnumerator(target));
  // }

  // public PSMeshRendererUpdater GetMeshUpdaterByName(GameObject target, string effectName) {
  //   var meshUpdaters = target.GetComponentsInChildren<PSMeshRendererUpdater>();
  //   // chech Contains since instantiated prefab may have extra suffix like "(Clone)"
  //   return Array.Find(meshUpdaters, element => element.gameObject.name.Contains(effectName));
  // }
}
#endif