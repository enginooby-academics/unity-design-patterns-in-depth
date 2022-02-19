#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TypeUtils;

namespace GOConstruction.Scripting {
  public sealed class ReflectionScriptingShapeGenerator : MonoBehaviour {
    [SerializeField]
    [LabelText("Current Type")]
    [ValueDropdown(nameof(GetTypeNames))]
    [OnValueChanged(nameof(UpdateCurrentQualifiedName))]
    private string _currentTypeName;

    private IEnumerable<string> _typeNames;

    private IEnumerable<string> _qualifiedNames;

    [SerializeField] [HideInInspector] private string _currentQualifiedName;

    // ! guard case: current type is removed
    public Type CurrentShapeType => Type.GetType(_currentQualifiedName) ?? GetAndSetFirstType();

    private void UpdateCurrentQualifiedName() {
      var id = _typeNames.IndexOf(_currentTypeName);
      _currentQualifiedName = _qualifiedNames.ElementAt(id);
    }

    private IEnumerable<string> GetTypeNames() {
      _qualifiedNames = GetConcreteTypeQualifiedNamesOf<IShape>();
      return _typeNames = GetConcreteTypeNamesOf<IShape>();
    }

    private Type GetAndSetFirstType() {
      GetTypeNames();
      _currentTypeName = _typeNames.ElementAt(0);
      _currentQualifiedName = _qualifiedNames.ElementAt(0);
      return Type.GetType(_currentQualifiedName);
    }

    public IShape CreateShapeInstance() {
      if (!CurrentShapeType.IsSubclassOf(typeof(MonoBehaviour)))
        return (IShape) Activator.CreateInstance(CurrentShapeType);

      return new GameObject().AddComponent(CurrentShapeType) as IShape;
    }

    [Button]
    public void CreateShape() {
      var shape = CreateShapeInstance();
      print("Volume of the generated shape is: " + shape.GetVolume());
    }
  }
}