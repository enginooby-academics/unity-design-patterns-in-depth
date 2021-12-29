using UnityEngine;

// ? MonoBehaviour -> always lazy init instance?

namespace SingletonPattern.Case2 {
  public abstract class MonoBehaviourCube : MonoBehaviour {
    // TIP: Use this property in replacement of a private field & a public getter property
    public float Size { get; protected set; }

    void Start() {
      // some visual stuffs...
      if (Size == 0) Size = Random.Range(1f, 5f);
      Setup(gameObject, Size);

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
