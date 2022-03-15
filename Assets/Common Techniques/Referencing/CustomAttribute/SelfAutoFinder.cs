using Enginooby.Attribute;
using UnityEngine;

namespace Techniques.Referencing {
  [RequireComponent(typeof(ITarget))] // less error-prone
  public class SelfAutoFinder : MonoBehaviour {
    [AutoRef(AutoRefTarget.Self)] private Target _target;

    private void Start() {
      print(_target is null ? "Target not found." : "Target found.");
    }
  }
}