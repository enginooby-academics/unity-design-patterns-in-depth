using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Shared = AbstractFactoryPattern.Case1;


namespace AbstractFactoryPattern.Case1.Naive {
  public class ClientShapeGenerator : Shared.ClientShapeGenerator {
    // ! need to modify if new theme is added
    public enum Theme { Simple, Shaking }

    [SerializeField, EnumToggleButtons]
    private Theme _currentTheme;

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
  }
}