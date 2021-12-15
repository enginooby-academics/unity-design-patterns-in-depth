using UnityEngine;

namespace AbstractFactoryPattern.Case1.Base {
  public class ShakingSphere : Sphere {
    /// * [A 'Concrete Product']

    public ShakingSphere() : base(Random.Range(1f, 2f), Color.blue) {
      _gameObject.transform.ShakePosition();
    }
  }
}
