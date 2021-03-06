using UnityEngine;

namespace Techniques.Referencing {
  public class NameFinder : MonoBehaviour {
    private void Awake() {
      var target = GameObject.Find("Target");
      print(target is null ? "Target not found." : "Target found.");
    }
  }
}