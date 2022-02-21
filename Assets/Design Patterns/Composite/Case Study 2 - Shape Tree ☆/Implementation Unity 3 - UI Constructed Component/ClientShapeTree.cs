using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace CompositePattern.Case2.Unity3 {
  public class ClientShapeTree : MonoBehaviourGizmos {
    [SerializeField] private IShapeContainer _rootPrefab;

    [ShowInInspector] private IShape _root;

    private void Awake() {
      _root = Instantiate(_rootPrefab.Object) as IShape;
      (_root as MonoBehaviour).transform.SetParent(transform);
      (_root as MonoBehaviour).gameObject.name = "Root";
    }
  }
}