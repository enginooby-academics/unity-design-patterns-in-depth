using UnityEngine;

namespace Reflection.Case1 {
  public class MonoBehaviourCube : MonoBehaviour, IShape {
    void Start() {
      gameObject.name = "MonoBehaviour Cube";
      gameObject.SetMaterialColor(Color.blue);
      gameObject.SetPrimitiveMesh(PrimitiveType.Cube);
    }

    public float GetVolume() => Mathf.Pow(1, 3);
  }
}
