using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable, InlineProperty]
/// <summary>
/// Concrete type collection of an abstract or interface.
/// </summary>
public class ReferenceType<T> {
  [SerializeField, HideLabel]
  [ValueDropdown(nameof(GetTypeNames)), OnValueChanged(nameof(UpdateCurrentType))]
  protected String _currentTypeName;

  private List<String> typeNames;
  private List<Type> types;
  private Type _currentType;

  // ! protect in case current type is removed
  public Type Value => _currentType ?? SetAndGetFirstType();

  // [Button]
  public virtual T CreateInstance() {
    return (T)Activator.CreateInstance(Value);
  }

  // private IEnumerable<String> GetTypeFullNames() {
  //   var typeFullNames = new List<String>();
  //   for (int i = 0; i < types.Count; i++) {
  //     typeFullNames.Add(types[i].AssemblyQualifiedName);
  //   }
  //   return typeFullNames;
  // }

  private IEnumerable<String> GetTypeNames() {
    types = TypeUtils.GetTypesOf<T>();
    typeNames = new List<String>();

    for (int i = 0; i < types.Count; i++) {
      typeNames.Add(types[i].Name);
    }
    return typeNames;
  }

  private void UpdateCurrentType() {
    _currentType = types[typeNames.IndexOf(_currentTypeName)];
  }

  protected Type SetAndGetFirstType() {
    types = TypeUtils.GetTypesOf<T>();
    _currentTypeName = types[0].Name;
    _currentType = types[0];
    return _currentType;
  }
}

[Serializable, InlineProperty]
/// <summary>
/// Concrete type collection of an abstract MonoBehaviour.
/// </summary>
public class ReferenceTypeMonoBehaviour<T> : ReferenceType<T> where T : Component {
  /// <summary>
  /// Create instance of given MonoBehaviour type with MeshFilter and MeshRenderer components
  /// </summary>
  // [Button]
  public override T CreateInstance() {
    return CreateInstanceWith(typeof(MeshFilter), typeof(MeshRenderer));
  }

  /// <summary>
  /// Create instance of given MonoBehaviour type with given component type list.
  /// </summary>
  // [Button]
  public T CreateInstanceWith(params Type[] components) {
    // var type = Type.GetType(_currentTypeName, true);
    var go = new GameObject();
    foreach (var component in components) {
      // if (component is Component)
      go.AddComponent(component);
    }

    var instance = go.AddComponent(Value);
    return (T)instance;
  }
}