using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AdapterPattern.Case1.Base1 {
  /// <summary>
  /// Individual color modifier for each GameObject.
  /// </summary>
  public class ClientColorModifier : MonoBehaviour {
    [SerializeField, HideLabel, OnValueChanged(nameof(UpdateColor))]
    private Color _color = Color.red;

    [SerializeReference, HideInInspector]
    private IColorizable _colorizableObject;

    private bool _isInitialized;

    private void Reset() {
      var colorizableTypes = TypeUtils.GetConcreteTypesOf<IColorizable>();
      colorizableTypes.ForEach(type => TrySetupWith(type));
      if (!_isInitialized) Debug.LogError($"There is no colorizable component/object on the GameObject.");
    }

    private void TrySetupWith(Type colorizableType) {
      if (_isInitialized) return;

      var colorizable = (IColorizable)Activator.CreateInstance(colorizableType);
      var componentType = colorizable.ComponentType;

      if (TryGetComponent(componentType, out var component)) {
        _colorizableObject = CreateInstance(colorizableType, component);
        _isInitialized = true;
      }
    }

    public IColorizable CreateInstance(Type colorizableType, params object[] paramArray) {
      return (IColorizable)Activator.CreateInstance(colorizableType, args: paramArray);
    }

    private void UpdateColor() {
      if (!_isInitialized) Reset();
      _colorizableObject.Color = _color;
    }

    public void SetColor(Color color) {
      _color = color;
      UpdateColor();
    }
  }
}
