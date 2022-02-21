using System;
using Enginooby.Utils;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace AdapterPattern.Case1.Base1 {
  /// <summary>
  ///   Individual color modifier for each GameObject.
  /// </summary>
  public class ClientColorModifier : MonoBehaviour {
    [SerializeField] [HideLabel] [OnValueChanged(nameof(UpdateColor))]
    private Color _color = Color.red;

    [SerializeReference] private IColorizable _colorizableObject;

    private bool _isInitialized;

    private void Reset() {
      var colorizableTypes = TypeUtils.GetConcreteTypesOf<IColorizable>();
      colorizableTypes.ForEach(TrySetupWith);
      if (!_isInitialized) Debug.LogError("There is no colorizable component/object on the GameObject.");
    }

    private void TrySetupWith(Type colorizableType) {
      if (_isInitialized) return;

      var colorizable = (IColorizable) Activator.CreateInstance(colorizableType);
      var componentType = colorizable.ComponentType;

      if (TryGetComponent(componentType, out var component)) {
        _colorizableObject = CreateInstance(colorizableType, component);
        _isInitialized = true;
      }
    }

    public IColorizable CreateInstance(Type colorizableType, params object[] paramArray) =>
      (IColorizable) Activator.CreateInstance(colorizableType, paramArray);

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