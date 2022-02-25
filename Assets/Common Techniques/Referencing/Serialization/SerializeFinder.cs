using UnityEngine;

namespace Techniques.Referencing {
  public class SerializeFinder : MonoBehaviour {
    [SerializeField] private Target _target;

    private void Start() {
      print(_target is null ? "Target not found." : "Target found.");
    }
  }
}