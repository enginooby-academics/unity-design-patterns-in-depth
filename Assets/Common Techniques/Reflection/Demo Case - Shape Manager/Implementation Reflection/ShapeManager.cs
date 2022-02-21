#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TypeUtils;

namespace Reflection.Case1.Reflection {
  public class ShapeManager : MonoBehaviour {
    [SerializeField]
    [LabelText("Shape Type")]
    [ValueDropdown(nameof(GetShapeTypeNames))]
    [OnValueChanged(nameof(UpdateCurrentType))]
    private string _currentShapeTypeName;

    private Type _currentShapeType;

    private IEnumerable<string> _shapeTypeNames;
    private IEnumerable<Type> _shapeTypes;

    // ! guard case: current type is removed
    public Type CurrentShapeType => _currentShapeType ?? SetAndGetFirstType();

    protected Type SetAndGetFirstType() {
      GetShapeTypeNames();
      _currentShapeTypeName = _shapeTypes.ElementAt(0).Name;
      _currentShapeType = _shapeTypes.ElementAt(0);
      return _currentShapeType;
    }

    private IEnumerable<string> GetShapeTypeNames() {
      _shapeTypes = GetConcreteTypesOf<IShape>();
      return _shapeTypeNames = GetConcreteTypeNamesOf<IShape>();
    }

    private void UpdateCurrentType() {
      var typeIndex = _shapeTypeNames.IndexOf(_currentShapeTypeName);
      _currentShapeType = _shapeTypes.ElementAt(typeIndex);
    }

    [Button]
    public void CreateShape() {
      IShape shape;

      if (!CurrentShapeType.IsSubclassOf(typeof(MonoBehaviour))) {
        shape = (IShape) Activator.CreateInstance(CurrentShapeType);
      }
      else {
        var go = new GameObject();
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>();
        shape = go.AddComponent(CurrentShapeType) as IShape;
      }

      print("Volume of the newly-created shape is: " + shape?.GetVolume());
    }
  }
}