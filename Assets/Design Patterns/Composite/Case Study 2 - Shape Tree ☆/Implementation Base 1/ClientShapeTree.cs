using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace CompositePattern.Case2.Base1 {
  public class ClientShapeTree : MonoBehaviourGizmos {
    [SerializeReference] [HideLabel] public IShape root;

    private void Awake() {
      root = new CompoundShape();
      root.GameObject.name = "Root";
      root.GameObject.transform.SetParent(transform);
    }

    public override void DrawGizmos() => (root as CompoundShape)?.DrawLink();
  }
}