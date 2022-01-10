using UnityEngine;

// TODO: Rename to FlattenableCollection

public class FlattenCollection : MonoBehaviour {
  public bool flatten = false;
  public bool removeInPlayMode = false; // TODO: enable on flatten is true

  private void Awake() {
    if (!flatten) return;

    Transform parent = transform.parent;
    int siblingIndex = transform.GetSiblingIndex();

    while (transform.childCount > 0) {
      Transform child = transform.GetChild(transform.childCount - 1);
      child.SetParent(parent, true);
      child.SetSiblingIndex(siblingIndex);
    }

    if (removeInPlayMode) Destroy(gameObject);
  }
}