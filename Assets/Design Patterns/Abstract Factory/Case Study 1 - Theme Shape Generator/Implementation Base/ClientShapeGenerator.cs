#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif
using UnityEngine;
using Shared = AbstractFactoryPattern.Case1;

namespace AbstractFactoryPattern.Case1.Base {
  /// <summary>
  ///   * [The 'Client' class]
  ///   Client instantiate different shapes by theme w/o specifying concrete shape classes.
  /// </summary>
  public class ClientShapeGenerator : Shared.ClientShapeGenerator {
    [SerializeReference] private ShapeFactory _currentShapeFactory;

    [Button]
    public void CreateCube() {
      var cube = _currentShapeFactory.CreateCube();
      cube.SetPos(RandomPos);
      _generatedCubes.Add(cube);
    }

    [Button]
    public void CreateSphere() {
      var sphere = _currentShapeFactory.CreateSphere();
      sphere.SetPos(RandomPos);
      _generatedSpheres.Add(sphere);
    }
  }
}