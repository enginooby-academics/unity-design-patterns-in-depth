using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using static TypeUtils;

namespace Reflection.Case1.Reflection {
  public class ShapeManager : MonoBehaviour {
    [SerializeField, LabelText("Shape Type")]
    [ValueDropdown(nameof(GetShapeTypeNames))]
    [OnValueChanged(nameof(UpdateCurrentType))]
    private String _currentShapeTypeName;

    private List<String> _shapeTypeNames;
    private List<Type> _shapeTypes;
    private Type _currentShapeType;

    // ! guard case: current type is removed
    public Type CurrentShapeType => _currentShapeType ?? SetAndGetFirstType();

    protected Type SetAndGetFirstType() {
      GetShapeTypeNames();
      _currentShapeTypeName = _shapeTypes[0].Name;
      _currentShapeType = _shapeTypes[0];
      return _currentShapeType;
    }

    private IEnumerable<String> GetShapeTypeNames() {
      _shapeTypes = GetConcreteTypesOf<IShape>();
      return _shapeTypeNames = GetConcreteTypeNamesOf<IShape>();
    }

    private void UpdateCurrentType() {
      int typeIndex = _shapeTypeNames.IndexOf(_currentShapeTypeName);
      _currentShapeType = _shapeTypes[typeIndex];
    }

    [Button]
    public void CreateShape() {
      IShape shape = default;

      if (!CurrentShapeType.IsSubclassOf(typeof(MonoBehaviour))) {
        shape = (IShape)Activator.CreateInstance(CurrentShapeType);
      } else {
        var go = new GameObject();
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>();
        shape = go.AddComponent(CurrentShapeType) as IShape;
      }

      print("Volume of the newly-created shape is: " + shape.GetVolume());
    }
  }
}
