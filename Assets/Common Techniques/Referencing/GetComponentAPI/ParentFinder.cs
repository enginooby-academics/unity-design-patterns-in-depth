using UnityEngine;

namespace Techniques.Referencing {
  public class ParentFinder : MonoBehaviour {
    private void Awake() {
      var target = GetComponentInParent<Target>();
      print(target is null ? "Target not found." : "Target found.");
    }

    [AutoRef(AutoRefTarget.Parent)] private Target _target;

    private void Start() {
      print(_target is null ? "Target not found." : "Target found.");
    }
  }
}