using UnityEngine;

namespace Techniques.Referencing {
  [RequireComponent(typeof(ITarget))] // less error-prone
  public class SelfFinder : MonoBehaviour {
    private void Awake() {
      var target = GetComponent<Target>(); // find by class
      // var target = GetComponent<ITarget>(); // find by interface
      print(target is null ? "Target not found." : "Target found.");
    }
  }
}