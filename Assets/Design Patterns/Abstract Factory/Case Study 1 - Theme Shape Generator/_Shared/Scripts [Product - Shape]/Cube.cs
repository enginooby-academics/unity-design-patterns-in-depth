using UnityEngine;
using static PrimitiveUtils;

namespace AbstractFactoryPattern.Case1 {
  /// <summary>
  ///   * An abstract product class *
  /// </summary>
public abstract class Cube {
  protected readonly GameObject _gameObject;
  protected readonly float _size;

  protected Cube(float size, Color color) {
    _size = size;
    _gameObject = CreatePrimitive(PrimitiveType.Cube, color);
    _gameObject.SetScale(_size);
  }

  public void SetPos(Vector3 pos) => _gameObject.transform.position = pos;

  public float GetDiagonal() => _size * Mathf.Sqrt(3);
}
}