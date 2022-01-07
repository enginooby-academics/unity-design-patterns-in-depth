using Drawing;
using UnityEngine;

namespace CompositePattern.Case2.Unity2 {
  public class ClientShapeTree : MonoBehaviourGizmos {
    [SerializeField]
    private IShapeContainer _rootPrefab;

    void Awake() {
      var root = Instantiate(_rootPrefab.Object) as MonoBehaviour;
      root.transform.SetParent(transform);
      root.gameObject.name = "Root";
    }
  }
}
