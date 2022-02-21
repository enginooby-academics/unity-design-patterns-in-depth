using UnityEngine;

public static class InstantiateUtils {
  public static GameObject Instantiate(
    this MonoBehaviour go,
    GameObject prefab,
    Quaternion? rotation = null,
    AxisFlag keepPrefabPos = AxisFlag.All,
    Transform parent = null) =>
    Instantiate(go.transform.position, prefab, rotation, keepPrefabPos, parent);

  public static GameObject Instantiate(
    Vector3 pos,
    GameObject prefab,
    Quaternion? rotation = null,
    AxisFlag keepPrefabPos = AxisFlag.All,
    Transform parent = null) {
    var rotToSpawn = rotation ?? Quaternion.identity;

    var instance = Object.Instantiate(prefab, pos, rotToSpawn);

    instance.transform.UpdatePosOnAxis(prefab.transform, keepPrefabPos);
    if (parent && instance) instance.transform.SetParent(parent);
    return instance;
  }
}