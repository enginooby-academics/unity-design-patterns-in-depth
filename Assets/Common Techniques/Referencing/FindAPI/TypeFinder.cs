using System.Linq;
using Enginooby.Utils;
using UnityEngine;

namespace Techniques.Referencing {
  public class TypeFinder : MonoBehaviour {
    private void Awake() {
      // find by class
      // var target = FindObjectOfType<Target>();
      // print(target is null ? "Target not found." : "Target found.");

      // find by interface
      var targets = FindObjectsOfType<MonoBehaviour>().OfType<ITarget>();
      print(targets.IsNullOrEmpty() ? "Target not found." : "Target found.");
    }
  }
}