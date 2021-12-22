using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Concrete type collection of an abstract or interface.
/// Can be used to quickly construct GameObject (procedurally, however, components are not tuned) when creating instance.
/// </summary>
[Serializable, InlineProperty]
public class ReferenceConcreteType<T> where T : class {
  [SerializeField, HideLabel]
  [ValueDropdown(nameof(GetTypeNames))]
  [OnValueChanged(nameof(UpdateCurrentType))]
  private String _currentTypeName;

  private List<String> _typeNames;
  private List<Type> _types;
  private Type _currentType;

  // ! guard case: current type is removed
  public Type Value => _currentType ?? SetAndGetFirstType();

  /// <summary>
  /// If current type is subclass of MonoBehaviour, create GameObject of the current type with given extra component types.
  /// </summary>
  public virtual T CreateInstance(params Type[] extraComponents) {
    if (!Value.IsSubclassOf(typeof(MonoBehaviour)))
      return (T)Activator.CreateInstance(Value);

    var go = new GameObject();

    // construct GameObject procedurally
    foreach (var component in extraComponents) {
      if (component.IsSubclassOf(typeof(Component)))
        go.AddComponent(component);
    }

    return go.AddComponent(Value) as T;
  }

  private IEnumerable<String> GetTypeNames() {
    _types = TypeUtils.GetConcreteTypesOf<T>();
    _typeNames = new List<String>();

    for (int i = 0; i < _types.Count; i++) {
      _typeNames.Add(_types[i].Name);
    }

    return _typeNames;
  }

  private void UpdateCurrentType() {
    _currentType = _types[_typeNames.IndexOf(_currentTypeName)];
  }

  protected Type SetAndGetFirstType() {
    _types = TypeUtils.GetConcreteTypesOf<T>();
    _currentTypeName = _types[0].Name;
    _currentType = _types[0];

    return _currentType;
  }
}