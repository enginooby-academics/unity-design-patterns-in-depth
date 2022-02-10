using UnityEngine;
using Shared = GOConstruction;

namespace GOConstruction.Scripting {
  public abstract class Cube : IShape {
    protected Shared.Cube _cubeComponent;
    protected GameObject _gameObject;
    public double GetVolume() => _cubeComponent.GetVolume();

    protected Cube() {
      _gameObject = new GameObject();
      _cubeComponent = _gameObject.AddComponent<Shared.Cube>();
    }
  }
}
