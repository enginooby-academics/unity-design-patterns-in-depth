using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using static TypeUtils;

namespace GOConstruction.Scripting {
  public class ReflectionScriptingShapeGenerator : MonoBehaviour {
    [SerializeField, LabelText("Current Type")]
    [ValueDropdown(nameof(GetTypeNames))]
    [OnValueChanged(nameof(UpdateCurrentQualifiedName))]
    private String _currentTypeName;

    [SerializeField, HideInInspector]
    private List<String> _typeNames;

    [SerializeField, HideInInspector]
    private List<String> _qualifiedNames;

    [SerializeField, HideInInspector]
    private String _currentQualifiedName;

    private void UpdateCurrentQualifiedName() {
      int id = _typeNames.IndexOf(_currentTypeName);
      _currentQualifiedName = _qualifiedNames[id];
    }

    private IEnumerable<String> GetTypeNames() {
      _qualifiedNames = GetConcreteTypeQualifiedNamesOf<IShape>();
      return _typeNames = GetConcreteTypeNamesOf<IShape>();
    }

    // ! guard case: current type is removed
    public Type CurrentShapeType => Type.GetType(_currentQualifiedName) ?? GetAndSetFirstType();

    private Type GetAndSetFirstType() {
      GetTypeNames();
      _currentTypeName = _typeNames[0];
      _currentQualifiedName = _qualifiedNames[0];
      return Type.GetType(_currentQualifiedName);
    }

    public virtual IShape CreateShapeInstance() {
      if (!CurrentShapeType.IsSubclassOf(typeof(MonoBehaviour)))
        return (IShape)Activator.CreateInstance(CurrentShapeType);

      return new GameObject().AddComponent(CurrentShapeType) as IShape;
    }

    [Button]
    public void CreateShape() {
      IShape shape = CreateShapeInstance();
      print("Volume of the generated shape is: " + shape.GetVolume());
    }
  }
}
