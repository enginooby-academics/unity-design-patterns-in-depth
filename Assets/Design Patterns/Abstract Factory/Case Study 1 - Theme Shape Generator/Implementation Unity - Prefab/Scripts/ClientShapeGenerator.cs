using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AbstractFactoryPattern.Case1.Unity.Prefab {
  /// <summary>
  /// * [The 'Client' class]
  /// Client instantiate different shapes by theme w/o specifying concrete shape classes.
  /// </summary>
  public class ClientShapeGenerator : MonoBehaviour {
    [SerializeField]
    // private ShapeFactory _currentShapeFactory;
    private ShapeFactoryNonPrefabProduct _currentShapeFactory;

    private List<Cube> generatedCubes = new List<Cube>();
    private List<Sphere> generatedSpheres = new List<Sphere>();
    private Vector3 RandomPos => new Vector3(24, 8, 0).RandomRange();

    [Button]
    public void CreateCube() {
      var cube = _currentShapeFactory.CreateCube();
      cube.SetPos(RandomPos);
      generatedCubes.Add(cube);
    }

    [Button]
    public void CreateSphere() {
      var sphere = _currentShapeFactory.CreateSphere();
      sphere.SetPos(RandomPos);
      generatedSpheres.Add(sphere);
    }

    [Button]
    public void GetTotalDiagonals() {
      float total = generatedCubes.Sum(cube => cube.GetDiagonal());
      print("Total diagonal of all generated cubes is: " + total);
    }

    [Button]
    public void GetTotalDiameters() {
      float total = generatedSpheres.Sum(sphere => sphere.GetDiameter());
      print("Total diameter of all generated spheres is: " + total);
    }
  }
}