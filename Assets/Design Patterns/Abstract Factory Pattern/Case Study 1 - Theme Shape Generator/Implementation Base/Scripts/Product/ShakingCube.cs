using UnityEngine;

namespace AbstractFactoryPattern.Case1.Base {
  public class ShakingCube : Cube {
    /// * [A 'Concrete Product']
    public ShakingCube() : base(Random.Range(1f, 2f), Color.blue) {
      _gameObject.transform.ShakePosition();
    }
  }
}
