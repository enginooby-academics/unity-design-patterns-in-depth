#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using System;
using System.Collections.Generic;
using UnityEngine;
using static TypeUtils;

/// <summary>
///   Concrete type collection of an abstract or interface.
///   Can be used to quickly construct GameObject (procedurally) when creating instance (however, components are not
///   tuned).
/// </summary>
[Serializable]
[InlineProperty]
public class ReferenceConcreteType<T> where T : class {
  [SerializeField] [HideLabel] [ValueDropdown(nameof(GetTypeNames))] [OnValueChanged(nameof(UpdateCurrentType))]
  private string _currentTypeName;

  [SerializeField] [HideInInspector] private List<string> _typeNames;

  [SerializeField] [HideInInspector] private List<string> _qualifiedTypeNames;

  [SerializeField] [HideInInspector] private string _currentQualifiedTypeName;


  // ! guard case: current type is removed
  public Type Value => Type.GetType(_currentQualifiedTypeName) ?? GetAndSetFirstType();
  public T ValueT => Type.GetType(_currentQualifiedTypeName) as T;

  public bool Is<K>() where K : T => Value == typeof(K);

  private Type GetAndSetFirstType() {
    GetTypeNames();
    _currentTypeName = _typeNames[0];
    _currentQualifiedTypeName = _qualifiedTypeNames[0];
    return Type.GetType(_currentQualifiedTypeName);
  }

  /// <summary>
  ///   If current type is MonoBehaviour, create GameObject of the current type with the given extra component types.
  /// </summary>
  public virtual T CreateInstance(params Type[] extraComponents) {
    if (!Value.IsSubclassOf(typeof(MonoBehaviour)))
      return (T) Activator.CreateInstance(Value);

    // scripting-construction
    var go = new GameObject();
    foreach (var component in extraComponents)
      if (component.IsSubclassOf(typeof(Component)))
        go.AddComponent(component);

    return go.AddComponent(Value) as T;
  }

  /// <summary>
  ///   Create instance by constructor with params for non-MonoBehaviour type.
  /// </summary>
  public virtual T CreateInstanceWithParams(params object[] paramArray) {
    if (!Value.IsSubclassOf(typeof(MonoBehaviour)))
      return (T) Activator.CreateInstance(Value, paramArray);

    // scripting-construction
    var go = new GameObject();
    // foreach (var component in extraComponents) {
    //   if (component.IsSubclassOf(typeof(Component)))
    //     go.AddComponent(component);
    // }

    return go.AddComponent(Value) as T;
  }

  private IEnumerable<string> GetTypeNames() {
    _qualifiedTypeNames = GetConcreteTypeQualifiedNamesOf<T>();
    return _typeNames = GetConcreteTypeNamesOf<T>();
  }

  private void UpdateCurrentType() {
    var id = _typeNames.IndexOf(_currentTypeName);
    _currentQualifiedTypeName = _qualifiedTypeNames[id];
  }
}