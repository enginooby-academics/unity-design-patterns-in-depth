using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using static TypeUtils;

/// <summary>
/// Concrete type collection of an abstract or interface.
/// Can be used to quickly construct GameObject (procedurally) when creating instance (however, components are not tuned).
/// </summary>
[Serializable, InlineProperty]
public class ReferenceConcreteType<T> where T : class {
  [SerializeField, HideLabel]
  [ValueDropdown(nameof(GetTypeNames))]
  [OnValueChanged(nameof(UpdateCurrentType))]
  private String _currentTypeName;
  [SerializeField, HideInInspector]
  private List<String> _typeNames;
  [SerializeField, HideInInspector]
  private List<String> _qualifiedTypeNames;
  [SerializeField, HideInInspector]
  private String _currentQualifiedTypeName;

  public bool Is<K>() where K : T => Value == typeof(K);


  // ! guard case: current type is removed
  public Type Value => Type.GetType(_currentQualifiedTypeName) ?? GetAndSetFirstType();
  public T ValueT => Type.GetType(_currentQualifiedTypeName) as T;

  private Type GetAndSetFirstType() {
    GetTypeNames();
    _currentTypeName = _typeNames[0];
    _currentQualifiedTypeName = _qualifiedTypeNames[0];
    return Type.GetType(_currentQualifiedTypeName);
  }

  /// <summary>
  /// If current type is MonoBehaviour, create GameObject of the current type with the given extra component types.
  /// </summary>
  public virtual T CreateInstance(params Type[] extraComponents) {
    if (!Value.IsSubclassOf(typeof(MonoBehaviour)))
      return (T)Activator.CreateInstance(Value);

    // scripting-construction
    var go = new GameObject();
    foreach (var component in extraComponents) {
      if (component.IsSubclassOf(typeof(Component)))
        go.AddComponent(component);
    }

    return go.AddComponent(Value) as T;
  }

  /// <summary>
  /// Create instance by constructor with params for non-MonoBehaviour type.
  /// </summary>
  public virtual T CreateInstanceWithParams(params object[] paramArray) {
    if (!Value.IsSubclassOf(typeof(MonoBehaviour)))
      return (T)Activator.CreateInstance(Value, args: paramArray);

    // scripting-construction
    var go = new GameObject();
    // foreach (var component in extraComponents) {
    //   if (component.IsSubclassOf(typeof(Component)))
    //     go.AddComponent(component);
    // }

    return go.AddComponent(Value) as T;
  }

  private IEnumerable<String> GetTypeNames() {
    _qualifiedTypeNames = GetConcreteTypeQualifiedNamesOf<T>();
    return _typeNames = GetConcreteTypeNamesOf<T>();
  }

  private void UpdateCurrentType() {
    int id = _typeNames.IndexOf(_currentTypeName);
    _currentQualifiedTypeName = _qualifiedTypeNames[id];
  }
}