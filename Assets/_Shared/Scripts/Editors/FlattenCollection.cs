using UnityEngine;

// TODO: Rename to FlattenableCollection

public class FlattenCollection : MonoBehaviour {
  public bool flatten;
  public bool removeInPlayMode; // TODO: enable on flatten is true

  private void Awake() {
    if (!flatten) return;

    var parent = transform.parent;
    var siblingIndex = transform.GetSiblingIndex();

    while (transform.childCount > 0) {
      var child = transform.GetChild(transform.childCount - 1);
      child.SetParent(parent, true);
      child.SetSiblingIndex(siblingIndex);
    }

    if (removeInPlayMode) Destroy(gameObject);
  }
}