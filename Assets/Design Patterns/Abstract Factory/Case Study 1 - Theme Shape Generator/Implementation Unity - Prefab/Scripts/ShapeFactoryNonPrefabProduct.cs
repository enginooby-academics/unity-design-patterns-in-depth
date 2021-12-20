using UnityEngine;

namespace AbstractFactoryPattern.Case1.Unity.Prefab {
  /// <summary>
  /// * [The 'MonoBehaviour Factory']
  /// Use reflection to retrieve concrete product type. 
  /// This factory is used in case each product is created only by its MonoBehaviour without extra setup.
  /// </summary>
  public class ShapeFactoryNonPrefabProduct : MonoBehaviour {
    [SerializeField]
    private ReferenceTypeMonoBehaviour<Cube> _cubeType;

    [SerializeField]
    private ReferenceTypeMonoBehaviour<Sphere> _sphereType;

    public Cube CreateCube() => _cubeType.CreateInstance();

    public Sphere CreateSphere() => _sphereType.CreateInstance();
  }
}