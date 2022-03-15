using Enginooby.Attribute;
using UnityEngine;

namespace Techniques.Referencing {
  public class ParentAutoFinder : MonoBehaviour {
    [AutoRef(AutoRefTarget.Parent)] private Target _target;

    private void Start() {
      print(_target is null ? "Target not found." : "Target found.");
    }
  }
}