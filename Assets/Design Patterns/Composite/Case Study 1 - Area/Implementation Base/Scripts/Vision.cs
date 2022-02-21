using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace CompositePattern.Case1.Base {
  public class Vision : MonoBehaviourGizmos {
    [SerializeField] private GameObject _target;

    [SerializeField] [SerializeReference] [HideLabel]
    private Area _vision;

    private void Reset() {
      _vision = new AreaCircular(gameObject, 5f, 90);
    }

    private void Update() {
      if (_vision.Contains(_target)) print("Seeing target.");
    }

    public override void DrawGizmos() {
      _vision.DrawGizmos();
    }
  }
}