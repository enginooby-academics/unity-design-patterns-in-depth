using Drawing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CompositePattern.Case2.Base1 {
  public class ClientShapeTree : MonoBehaviourGizmos {
    [SerializeReference, HideLabel]
    public IShape root = null;

    void Awake() {
      root = new CompoundShape();
      root.GameObject.name = "Root";
      root.GameObject.transform.SetParent(transform);
    }

    public override void DrawGizmos() => (root as CompoundShape)?.DrawLink();
  }
}
