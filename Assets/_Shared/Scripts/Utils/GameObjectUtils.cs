using UnityEngine;
using System.Collections.Generic;

public static class GameObjectUtils {
  public static void ToggleActive(this GameObject go) {
    go.SetActive(!go.activeInHierarchy); // ? activeSelf or activeInHierarchy
  }

  public static void ToggleActive(this List<GameObject> goList) {
    goList.ForEach(ToggleActive);
  }
}