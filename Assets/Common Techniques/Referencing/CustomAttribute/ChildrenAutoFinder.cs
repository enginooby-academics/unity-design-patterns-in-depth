using UnityEngine;

namespace Techniques.Referencing {
  public class ChildrenAutoFinder : MonoBehaviour {
    [AutoRef(AutoRefTarget.Children)] private Target _target;

    private void Start() {
      print(_target is null ? "Target not found." : "Target found.");
    }
  }
}