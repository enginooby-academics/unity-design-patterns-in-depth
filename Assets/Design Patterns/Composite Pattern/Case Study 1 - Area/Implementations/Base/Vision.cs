using Drawing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CompositePattern.Case1.Base {
  public class Vision : MonoBehaviourGizmos {
    [SerializeField]
    private GameObject _target;

    [SerializeField, SerializeReference, HideLabel]
    private Area _vision;

    private void Reset() {
      _vision = new AreaCircular(gameObject, radius: 5f, angle: 90);
    }

    public override void DrawGizmos() {
      _vision.DrawGizmos();
    }

    void Update() {
      if (_vision.Contains(_target)) {
        print("Seeing target.");
      }
    }
  }
}
