using UnityEngine;

namespace AbstractFactoryPattern.Case1.Unity.Prefab {
  /// <summary>
  /// * [A 'Concrete Product']
  /// </summary>
  public class SimpleCube : Cube {
    private void Awake() {
      _size = Random.Range(3f, 4f);
      gameObject.SetMaterialColor(Color.red);
    }
  }
}
