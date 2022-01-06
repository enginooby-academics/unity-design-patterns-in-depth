using Drawing;
using UnityEngine;

namespace CompositePattern.Case2.Unity1 {
  public class ClientShapeTree : MonoBehaviourGizmos {
    void Awake() {
      IShape root = new GameObject().AddComponent<CompoundShape>();
      (root as MonoBehaviour).gameObject.name = "Root";
      (root as MonoBehaviour).transform.SetParent(transform);
    }
  }
}
