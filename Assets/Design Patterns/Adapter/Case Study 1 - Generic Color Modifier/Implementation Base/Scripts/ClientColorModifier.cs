using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AdapterPattern.Case1.Base1 {
  public class ClientColorModifier : MonoBehaviour {
    [SerializeReference, HideInInspector]
    private IColorizable _colorizableObject;

    [SerializeField]
    private Color _color = Color.red;

    private bool _isInitialized;

    private void Reset() {
      var colorizableTypes = TypeUtils.GetConcreteTypesOf<IColorizable>();
      colorizableTypes.ForEach(type => TrySetupWith(type));
    }

    private void TrySetupWith(Type colorizableType) {
      if (_isInitialized) return;

      var colorizableInstance = (IColorizable)Activator.CreateInstance(colorizableType);
      var componentType = colorizableInstance.ComponentType;

      if (TryGetComponent(componentType, out var component)) {
        _colorizableObject = CreateInstance(colorizableType, component);
        _isInitialized = true;
        Debug.Log($"Component {componentType.ToString()} retrieved.");
      } else {
        Debug.LogError($"Component {componentType.ToString()} not found.");
      }
    }

    public IColorizable CreateInstance(Type colorizableType, params object[] paramArray) {
      return (IColorizable)Activator.CreateInstance(colorizableType, args: paramArray);
    }

    [Button]
    public void UpdateColor() {
      if (!_isInitialized) Reset();
      _colorizableObject.Color = _color;
    }

    public void SetColor(Color color) {
      _color = color;
      UpdateColor();
    }
  }
}
