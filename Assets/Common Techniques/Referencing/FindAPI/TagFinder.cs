using UnityEngine;

namespace Techniques.Referencing {
  public class TagFinder : MonoBehaviour {
    private void Awake() {
      var target = GameObject.FindWithTag("Target");
      print(target is null ? "Target not found." : "Target found.");
    }
  }
}