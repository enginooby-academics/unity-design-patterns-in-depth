#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginooby.Attribute;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using Enginooby.Utils;
using UnityEngine;
using static TypeUtils;

namespace GOConstruction.Scripting {
  public sealed class DecoupledScriptingShapeGenerator : MonoBehaviour {
    [SerializeField]
    [LabelText("Current Type")]
    [ValueDropdown(nameof(GetTypeNames))]
    [OnValueChanged(nameof(UpdateCurrentQualifiedName))]
    private string _currentTypeName;

    [SerializeField] [HideInInspector] private string _currentQualifiedName;

    private IEnumerable<string> _qualifiedNames;
    private IEnumerable<string> _typeNames;

    private IEnumerable<string> GetTypeNames() {
      _qualifiedNames = GetConcreteTypeQualifiedNamesOf<IShape>();
      return _typeNames = GetConcreteTypeNamesOf<IShape>();
    }

    private void UpdateCurrentQualifiedName() {
      var id = _typeNames.IndexOf(_currentTypeName);
      _currentQualifiedName = _qualifiedNames.ElementAt(id);
    }

    // ! guard case: current type is removed
    public Type CurrentShapeType => Type.GetType(_currentQualifiedName) ?? GetAndSetFirstType();

    private Type GetAndSetFirstType() {
      GetTypeNames();
      _currentTypeName = _typeNames.ElementAt(0);
      _currentQualifiedName = _qualifiedNames.ElementAt(0);
      return Type.GetType(_currentQualifiedName);
    }

    [Button]
    public void CreateShape() {
      var shape = CreateShapeInstance();
      print("Volume of the generated shape is: " + shape.GetVolume());
    }

    public IShape CreateShapeInstance() {
      if (!CurrentShapeType.IsSubclassOf(typeof(MonoBehaviour)))
        return (IShape) Activator.CreateInstance(CurrentShapeType);

      return new GameObject().AddComponent(CurrentShapeType) as IShape;
    }
  }
}