using UnityEngine;

namespace Techniques.Referencing {
  public class ChildrenFinder : MonoBehaviour {
    private void Awake() {
      var target = GetComponentInChildren<Target>();
      print(target is null ? "Target not found." : "Target found.");
    }
  }
}