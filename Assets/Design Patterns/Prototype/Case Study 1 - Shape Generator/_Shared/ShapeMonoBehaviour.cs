using UnityEngine;

namespace Prototype {
  /// <summary>
  ///   Add Unity behaviours to generated procedural shapes.
  /// </summary>
  public class ShapeMonoBehaviour : MonoBehaviour {
    public ProceduralShape shape;

    private void Update() {
      shape?.OnUpdate();
    }

    private void OnMouseDown() {
      FindObjectOfType<ShapeGenerator>().template = shape;
    }
  }
}