using UnityEngine;

namespace Prototype.Naive {
  /// <summary>
  /// Add Unity behaviours to generated procedural shapes.
  /// </summary>
  public class ShapeMonoBehaviour : MonoBehaviour {
    public ProceduralShape shape;

    void Start() {

    }

    void Update() {
      shape.OnUpdate();
    }

    private void OnMouseDown() {
      ShapeGenerator.Instance.template = shape;
    }
  }
}
