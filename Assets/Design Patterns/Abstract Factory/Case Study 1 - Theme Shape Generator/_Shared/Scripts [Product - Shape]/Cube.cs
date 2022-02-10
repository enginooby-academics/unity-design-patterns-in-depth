using UnityEngine;
using static PrimitiveUtils;

namespace AbstractFactoryPattern.Case1 {
  /// <summary>
  /// * [An 'Abstract Product']
  /// </summary>
  public abstract class Cube {
    protected float _size;
    protected GameObject _gameObject;

    protected Cube(float size, Color color) {
      _size = size;
      _gameObject = CreatePrimitive(PrimitiveType.Cube, color);
      _gameObject.SetScale(_size);
    }

    public void SetPos(Vector3 pos) => _gameObject.transform.position = pos;

    public float GetDiagonal() => _size * Mathf.Sqrt(3);
  }
}
