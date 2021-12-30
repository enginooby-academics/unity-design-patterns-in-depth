using UnityEngine;
using static UnityEngine.Mathf;

namespace GOConstruction {
  [DisallowMultipleComponent]
  public class Cube : MonoBehaviour, IShape {
    [Range(1f, 5f)]
    public float Size = 1f;

    public double GetVolume() => Pow(Size, 3);

    protected virtual void Awake() {
      gameObject.AddComponent<MeshFilter>();
      gameObject.AddComponent<MeshRenderer>();
      gameObject.SetPrimitiveMesh(PrimitiveType.Cube);
      gameObject.SetMaterialColor(Color.red);
      gameObject.SetScale(Size);
    }
  }
}