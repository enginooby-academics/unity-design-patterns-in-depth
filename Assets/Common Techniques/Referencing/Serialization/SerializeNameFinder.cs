using UnityEngine;

namespace Techniques.Referencing {
  public class SerializeNameFinder : MonoBehaviour {
    [SerializeField] private string _targetName;

    private void Start() {
      var target = GameObject.Find(_targetName);
      Debug.Log(target is null ? "Target not found." : "Target found.", this);
    }
  }
}