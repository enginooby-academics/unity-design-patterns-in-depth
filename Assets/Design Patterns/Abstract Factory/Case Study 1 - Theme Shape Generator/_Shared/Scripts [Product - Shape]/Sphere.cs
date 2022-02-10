using UnityEngine;
using static PrimitiveUtils;

namespace AbstractFactoryPattern.Case1 {
  /// <summary>
  /// * [An 'Abstract Product']
  /// </summary>
  public abstract class Sphere {
    protected float _radius;
    protected GameObject _gameObject;

    protected Sphere(float radius, Color color) {
      _radius = radius;
      _gameObject = CreatePrimitive(PrimitiveType.Sphere, color);
      _gameObject.SetScale(_radius);
    }

    public void SetPos(Vector3 pos) => _gameObject.transform.position = pos;

    public float GetDiameter() => _radius * 2;
  }
}
