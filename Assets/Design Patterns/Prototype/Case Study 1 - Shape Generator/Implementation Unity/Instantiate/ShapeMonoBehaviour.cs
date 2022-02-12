using UnityEngine;

namespace Prototype.Unity.Instantiate {
  /// <summary>
  ///   Add Unity behaviours to generated procedural shapes.
  /// </summary>
  public class ShapeMonoBehaviour : MonoBehaviour {
    private void OnMouseDown() {
      FindObjectOfType<ShapeGenerator>().templateGameObject = gameObject;
    }
  }
}