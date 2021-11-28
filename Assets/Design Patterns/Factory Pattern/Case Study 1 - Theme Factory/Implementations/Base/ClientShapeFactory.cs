using Sirenix.OdinInspector;
using UnityEngine;

namespace Factory.Base {
  /// <summary>
  /// Client instantiate different shapes by theme w/o specifying how to create each shape.
  /// </summary>
  public class ClientShapeFactory : MonoBehaviour {
    public enum ShapeTheme { Green, Red }

    [SerializeField, EnumToggleButtons, OnValueChanged(nameof(GetShapeFactory))]
    private ShapeTheme _theme;

    private void GetShapeFactory() {
      if (_theme == ShapeTheme.Green) {
        _currentShapeFactory = FindObjectOfType<GreenShapeFactory>();
      } else {
        _currentShapeFactory = FindObjectOfType<RedShapeFactory>();
      }
    }

    private ShapeFactory _currentShapeFactory;
    public ShapeFactory CurrentShapeFactory {
      get {
        if (!_currentShapeFactory) GetShapeFactory();
        return _currentShapeFactory;
      }
    }

    [Button]
    public GameObject SpawnCube() {
      return CurrentShapeFactory.SpawnCube();
    }

    [Button]
    public GameObject SpawnSphere() {
      return CurrentShapeFactory.SpawnSphere();
    }

    [Button]
    public GameObject SpawnCylinder() {
      return CurrentShapeFactory.SpawnCylinder();
    }
  }
}