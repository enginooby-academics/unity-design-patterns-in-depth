using UnityEngine;

namespace SingletonPattern.Case2 {
  public class Cube : MonoBehaviour {
    protected float _size;
    public float Size => _size;

    private void Awake() {
      _size = Random.Range(1f, 5f);
    }

    void Start() {
      transform.ResetPosition();
      gameObject.TryAddComponent<MeshFilter>();
      gameObject.TryAddComponent<MeshRenderer>();
      gameObject.SetPrimitiveMesh(PrimitiveType.Cube);
      gameObject.SetMaterialColor(Random.ColorHSV());
      gameObject.SetScale(_size);
    }
  }
}
