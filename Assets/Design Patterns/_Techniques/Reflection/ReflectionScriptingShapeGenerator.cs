#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using System;
using System.Collections.Generic;
using UnityEngine;
using static TypeUtils;

namespace GOConstruction.Scripting {
  public class ReflectionScriptingShapeGenerator : MonoBehaviour {
    [SerializeField]
    [LabelText("Current Type")]
    [ValueDropdown(nameof(GetTypeNames))]
    [OnValueChanged(nameof(UpdateCurrentQualifiedName))]
    private string _currentTypeName;

    [SerializeField] [HideInInspector] private List<string> _typeNames;

    [SerializeField] [HideInInspector] private List<string> _qualifiedNames;

    [SerializeField] [HideInInspector] private string _currentQualifiedName;

    // ! guard case: current type is removed
    public Type CurrentShapeType => Type.GetType(_currentQualifiedName) ?? GetAndSetFirstType();

    private void UpdateCurrentQualifiedName() {
      var id = _typeNames.IndexOf(_currentTypeName);
      _currentQualifiedName = _qualifiedNames[id];
    }

    private IEnumerable<string> GetTypeNames() {
      _qualifiedNames = GetConcreteTypeQualifiedNamesOf<IShape>();
      return _typeNames = GetConcreteTypeNamesOf<IShape>();
    }

    private Type GetAndSetFirstType() {
      GetTypeNames();
      _currentTypeName = _typeNames[0];
      _currentQualifiedName = _qualifiedNames[0];
      return Type.GetType(_currentQualifiedName);
    }

    public virtual IShape CreateShapeInstance() {
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