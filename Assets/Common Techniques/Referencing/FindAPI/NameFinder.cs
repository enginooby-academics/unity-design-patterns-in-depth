using UnityEngine;

namespace Techniques.Referencing {
  public class NameFinder : MonoBehaviour {
    private void Awake() {
      var target = GameObject.Find("Target");
      print(target is not null ? "Target found." : "Target not found.");
    }
  }
}