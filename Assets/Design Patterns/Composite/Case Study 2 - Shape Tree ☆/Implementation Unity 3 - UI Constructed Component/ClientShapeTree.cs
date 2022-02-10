using Drawing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CompositePattern.Case2.Unity3 {
  public class ClientShapeTree : MonoBehaviourGizmos {
    [SerializeField]
    private IShapeContainer _rootPrefab;

    [ShowInInspector]
    private IShape _root;

    void Awake() {
      _root = Instantiate(_rootPrefab.Object) as IShape;
      (_root as MonoBehaviour).transform.SetParent(transform);
      (_root as MonoBehaviour).gameObject.name = "Root";
    }
  }
}
