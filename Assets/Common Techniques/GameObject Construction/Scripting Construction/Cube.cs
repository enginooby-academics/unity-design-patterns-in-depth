using UnityEngine;
using Shared = GOConstruction;

namespace GOConstruction.Scripting {
  public abstract class Cube : IShape {
    protected readonly GameObject _gameObject;
    private readonly Shared.Cube _cubeComponent;

    protected Cube() {
      _gameObject = new GameObject();
      _cubeComponent = _gameObject.AddComponent<Shared.Cube>();
    }

    public double GetVolume() => _cubeComponent.GetVolume();
  }
}