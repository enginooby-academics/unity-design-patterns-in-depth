using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AbstractFactoryPattern.Case1.Naive {
  public class ClientShapeGenerator : MonoBehaviour {
    // ! need to modify if new theme is added
    public enum Theme { Simple, Shaking }

    [SerializeField, EnumToggleButtons]
    private Theme _currentTheme;

    private List<Cube> generatedCubes = new List<Cube>();
    private List<Sphere> generatedSpheres = new List<Sphere>();
    private Vector3 RandomPos => new Vector3(24, 8, 0).RandomRange();

    [Button]
    public void CreateCube() {
      // ! tightly coupled w/ concrete product classes
      Cube cube = _currentTheme switch
      {
        Theme.Simple => new SimpleCube(),
        Theme.Shaking => new ShakingCube(),
        _ => throw new ArgumentOutOfRangeException(),
      };
      cube.SetPos(RandomPos);
      generatedCubes.Add(cube);
    }

    [Button]
    public void CreateSphere() {
      Sphere sphere = _currentTheme switch
      {
        Theme.Simple => new SimpleSphere(),
        Theme.Shaking => new ShakingSphere(),
        _ => throw new ArgumentOutOfRangeException(),
      };
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