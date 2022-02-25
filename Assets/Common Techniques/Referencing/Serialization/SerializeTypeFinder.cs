using UnityEngine;

namespace Techniques.Referencing {
  public class SerializeTypeFinder : MonoBehaviour {
    [SerializeField] private ReferenceConcreteType<ITarget> _targetType;

    private void Start() {
      var target = FindObjectOfType(_targetType.Value);
      Debug.Log(target is null ? "Target not found." : "Target found.", this);
    }
  }
}