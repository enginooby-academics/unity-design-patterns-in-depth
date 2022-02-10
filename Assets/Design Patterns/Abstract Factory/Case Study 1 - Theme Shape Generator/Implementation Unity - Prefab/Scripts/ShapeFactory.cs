using UnityEngine;

namespace AbstractFactoryPattern.Case1.Unity.Prefab {
  /// <summary>
  /// * [The 'MonoBehaviour Factory']
  /// Use product prefabs
  /// </summary>
  public class ShapeFactory : MonoBehaviour {
    [SerializeField]
    private Cube _cubePrefab;

    [SerializeField]
    private Sphere _spherePrefab;

    public Cube CreateCube() => Instantiate(_cubePrefab);

    public Sphere CreateSphere() => Instantiate(_spherePrefab);
  }
}