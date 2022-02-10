using UnityEngine;
using Shared = GOConstruction;

namespace GOConstruction.Scripting {
  public abstract class Sphere : IShape {
    protected Shared.Sphere _sphereComponent;
    protected GameObject _gameObject;
    public double GetVolume() => _sphereComponent.GetVolume();

    protected Sphere() {
      _gameObject = new GameObject();
      _sphereComponent = _gameObject.AddComponent<Shared.Sphere>();
    }
  }
}
