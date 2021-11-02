using UnityEngine;
public static class InstantiateUtils {
  public static GameObject Instantiate(this MonoBehaviour go, GameObject prefab, Quaternion? rotation = null, AxisFlag keepPrefabPos = AxisFlag.ALL, Transform parent = null) {
    return InstantiateUtils.Instantiate(go.transform.position, prefab: prefab, rotation: rotation, keepPrefabPos: keepPrefabPos, parent: parent);
  }

  public static GameObject Instantiate(Vector3 pos, GameObject prefab, Quaternion? rotation = null, AxisFlag keepPrefabPos = AxisFlag.ALL, Transform parent = null) {
    Quaternion rotToSpawn = (rotation == null) ? Quaternion.identity : rotation.Value;

    GameObject instance = Object.Instantiate(prefab, pos, rotToSpawn);

    instance.transform.UpdatePosOnAxis(target: prefab.transform, axis: keepPrefabPos);
    if (parent && instance) instance.transform.SetParent(parent);
    return instance;
  }
}