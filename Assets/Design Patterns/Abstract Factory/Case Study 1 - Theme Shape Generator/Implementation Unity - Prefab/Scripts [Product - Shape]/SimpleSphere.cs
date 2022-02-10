using UnityEngine;

namespace AbstractFactoryPattern.Case1.Unity.Prefab {
  /// <summary>
  /// * [A 'Concrete Product']
  /// </summary>
  public class SimpleSphere : Sphere {
    private void Awake() {
      _radius = Random.Range(3f, 4f);
      gameObject.SetMaterialColor(Color.red);
    }
  }
}
