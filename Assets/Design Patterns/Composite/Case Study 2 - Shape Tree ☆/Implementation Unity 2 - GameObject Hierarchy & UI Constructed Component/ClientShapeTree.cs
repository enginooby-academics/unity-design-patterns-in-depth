using Drawing;
using UnityEngine;

namespace CompositePattern.Case2.Unity2 {
  public class ClientShapeTree : MonoBehaviourGizmos {
    [SerializeField]
    private IShapeContainer _rootPrefab;

    void Awake() {
      IShape root = Instantiate(_rootPrefab.Object) as IShape;
      (root as MonoBehaviour).transform.SetParent(transform);
      (root as MonoBehaviour).gameObject.name = "Root";
    }
  }
}
