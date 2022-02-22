using UnityEngine;

namespace Techniques.Referencing {
  public class ChildrenFinder : MonoBehaviour {
    private void Awake() {
      var target = GetComponentInChildren<Target>();
      print(target is null ? "Target not found." : "Target found.");
    }

    [AutoRef(AutoRefTarget.Children)] private Target _target;

    private void Start() {
      print(_target is null ? "Target not found." : "Target found.");
    }
  }
}