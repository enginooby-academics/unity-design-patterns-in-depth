using UnityEngine;

namespace AbstractFactoryPattern.Case1.Unity.Prefab {
  /// <summary>
  /// * [A 'Concrete Product']
  /// </summary>
  public class ShakingSphere : Sphere {
    private void Awake() {
      _radius = Random.Range(1f, 2f);
      gameObject.SetMaterialColor(Color.blue);
      transform.ShakePosition();
    }
  }
}
