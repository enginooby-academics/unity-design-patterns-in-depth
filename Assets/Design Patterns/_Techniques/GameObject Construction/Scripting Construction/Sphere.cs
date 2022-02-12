using UnityEngine;
using Shared = GOConstruction;

namespace GOConstruction.Scripting {
  public abstract class Sphere : IShape {
    protected GameObject _gameObject;
    protected Shared.Sphere _sphereComponent;

    protected Sphere() {
      _gameObject = new GameObject();
      _sphereComponent = _gameObject.AddComponent<Shared.Sphere>();
    }

    public double GetVolume() => _sphereComponent.GetVolume();
  }
}