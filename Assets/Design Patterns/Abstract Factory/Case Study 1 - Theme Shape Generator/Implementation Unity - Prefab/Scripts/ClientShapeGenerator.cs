using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace AbstractFactoryPattern.Case1.Unity.Prefab {
  /// <summary>
  ///   * [The 'Client' class]
  ///   Client instantiate different shapes by theme w/o specifying concrete shape classes.
  /// </summary>
  public class ClientShapeGenerator : MonoBehaviour {
    [SerializeField] private ShapeFactory _currentShapeFactory;
    // private ProceduralShapeFactory _currentShapeFactory;

    private readonly List<Cube> _generatedCubes = new();
    private readonly List<Sphere> _generatedSpheres = new();
    private Vector3 RandomPos => new Vector3(24, 8, 0).RandomRange();

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

    [Button]
    public void GetTotalDiagonals() {
      var total = _generatedCubes.Sum(cube => cube.GetDiagonal());
      print("Total diagonal of all generated cubes is: " + total);
    }

    [Button]
    public void GetTotalDiameters() {
      var total = _generatedSpheres.Sum(sphere => sphere.GetDiameter());
      print("Total diameter of all generated spheres is: " + total);
    }
  }
}