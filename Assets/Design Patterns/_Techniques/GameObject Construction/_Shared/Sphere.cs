using UnityEngine;
using static UnityEngine.Mathf;

namespace GOConstruction {
  [DisallowMultipleComponent]
  public class Sphere : MonoBehaviour, IShape {
    [Range(1f, 5f)]
    public float Radius = 1f;

    public double GetVolume() => 4 / 3 * PI * Pow(Radius, 3);

    protected virtual void Awake() {
      gameObject.AddComponent<MeshFilter>();
      gameObject.AddComponent<MeshRenderer>();
      gameObject.SetPrimitiveMesh(PrimitiveType.Sphere);
      gameObject.SetMaterialColor(Color.blue);
      gameObject.SetScale(Radius);
    }
  }
}