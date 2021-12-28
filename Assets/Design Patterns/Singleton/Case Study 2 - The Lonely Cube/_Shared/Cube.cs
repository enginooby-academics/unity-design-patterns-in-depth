using UnityEngine;

// ? MonoBehaviour -> always lazy init instance?

namespace SingletonPattern.Case2 {
  public class Cube : MonoBehaviour {
    protected float _size;
    public float Size => _size;

    void Start() {
      if (_size == 0) _size = Random.Range(1f, 5f);
      Setup(gameObject, _size);
    }

    /// <summary>
    /// Separate static function for reusing without inheritance in generic singleton.
    /// </summary>
    public static void Setup(GameObject go, float size) {
      go.transform.ResetPosition();
      go.TryAddComponent<MeshFilter>();
      go.TryAddComponent<MeshRenderer>();
      go.SetPrimitiveMesh(PrimitiveType.Cube);
      go.SetMaterialColor(Random.ColorHSV());
      go.SetScale(size);
    }
  }
}
