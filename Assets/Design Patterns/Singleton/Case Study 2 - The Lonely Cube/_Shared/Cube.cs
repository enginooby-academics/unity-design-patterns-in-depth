using UnityEngine;

// ? MonoBehaviour -> always lazy init instance?

namespace SingletonPattern.Case2 {
  public class Cube : MonoBehaviour {
    protected float _size;
    public float Size => _size;

    void Start() {
      if (_size == 0) _size = Random.Range(1f, 5f);

      transform.ResetPosition();
      gameObject.TryAddComponent<MeshFilter>();
      gameObject.TryAddComponent<MeshRenderer>();
      gameObject.SetPrimitiveMesh(PrimitiveType.Cube);
      gameObject.SetMaterialColor(Random.ColorHSV());
      gameObject.SetScale(_size);
    }
  }
}
