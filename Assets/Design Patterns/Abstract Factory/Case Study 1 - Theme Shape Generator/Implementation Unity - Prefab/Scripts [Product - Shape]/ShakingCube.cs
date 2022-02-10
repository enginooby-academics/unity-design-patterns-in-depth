using UnityEngine;

namespace AbstractFactoryPattern.Case1.Unity.Prefab {
  /// <summary>
  /// * [A 'Concrete Product']
  /// </summary>
  public class ShakingCube : Cube {
    private void Awake() {
      _size = Random.Range(1f, 2f);
      gameObject.SetMaterialColor(Color.blue);
      transform.ShakePosition();
    }
  }
}
