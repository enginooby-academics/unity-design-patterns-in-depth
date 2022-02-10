using UnityEngine;

namespace AbstractFactoryPattern.Case1.Unity.Prefab {
  /// <summary>
  /// * [The 'MonoBehaviour Factory']
  /// Use reflection to retrieve concrete product type. 
  /// Construct GameObject products procedurally.
  /// </summary>
  public class ProceduralShapeFactory : MonoBehaviour {
    [SerializeField]
    private ReferenceConcreteType<Cube> _cubeType;

    [SerializeField]
    private ReferenceConcreteType<Sphere> _sphereType;

    public Cube CreateCube() => _cubeType.CreateInstance(typeof(MeshFilter), typeof(MeshRenderer));

    public Sphere CreateSphere() => _sphereType.CreateInstance(typeof(MeshFilter), typeof(MeshRenderer));
  }
}