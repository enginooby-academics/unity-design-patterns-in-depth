using UnityEngine;

namespace Reflection.Case1 {
  public class MonoBehaviourSphere : MonoBehaviour, IShape {
    void Start() {
      gameObject.name = "MonoBehaviour Sphere";
      gameObject.SetMaterialColor(Color.red);
      gameObject.SetPrimitiveMesh(PrimitiveType.Sphere);
    }

    public float GetVolume() => 4 / 3 * Mathf.PI * Mathf.Pow(1, 3);
  }
}
