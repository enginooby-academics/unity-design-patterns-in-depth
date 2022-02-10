using UnityEngine;

namespace AbstractFactoryPattern.Case1 {
  /// <summary>
  /// * [A 'Concrete Product']
  /// </summary>
  public class ShakingCube : Cube {
    public ShakingCube() : base(Random.Range(1f, 2f), Color.blue) {
      _gameObject.transform.ShakePosition();
    }
  }
}
