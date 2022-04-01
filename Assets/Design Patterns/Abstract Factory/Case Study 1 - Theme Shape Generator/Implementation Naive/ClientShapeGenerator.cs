#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif
using System;
using UnityEngine;
using Shared = AbstractFactoryPattern.Case1;

namespace AbstractFactoryPattern.Case1.Naive {
  public class ClientShapeGenerator : Shared.ClientShapeGenerator {
    [EnumToggleButtons] [SerializeField] private Theme _currentTheme;

    [Button]
    public void CreateCube() {
      // ! tightly coupled w/ concrete product classes
      Cube cube = _currentTheme switch {
        Theme.Simple => new SimpleCube(),
        Theme.Shaking => new ShakingCube(),
        _ => throw new ArgumentOutOfRangeException(),
      };
      cube.SetPos(RandomPos);
      _generatedCubes.Add(cube);
    }

    [Button]
    public void CreateSphere() {
      Sphere sphere = _currentTheme switch {
        Theme.Simple => new SimpleSphere(),
        Theme.Shaking => new ShakingSphere(),
        _ => throw new ArgumentOutOfRangeException(),
      };
      sphere.SetPos(RandomPos);
      _generatedSpheres.Add(sphere);
    }

    // ! need to modify if new theme is added
    private enum Theme {
      Simple,
      Shaking,
    }
  }
}