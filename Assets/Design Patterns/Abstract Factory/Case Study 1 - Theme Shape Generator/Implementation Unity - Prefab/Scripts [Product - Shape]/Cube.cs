using UnityEngine;

namespace AbstractFactoryPattern.Case1.Unity.Prefab {
  /// <summary> 
  /// * [An 'Abstract Product'] 
  /// </summary> 
  public abstract class Cube : MonoBehaviour {
    protected float _size;
    public void SetPos(Vector3 pos) => transform.position = pos;
    public float GetDiagonal() => _size * Mathf.Sqrt(3);

    private void Start() {
      transform.SetScale(_size);
      gameObject.SetPrimitiveMesh(PrimitiveType.Cube);
    }
  }
}