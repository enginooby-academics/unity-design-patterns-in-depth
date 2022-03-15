// Credit: https://assetstore.unity.com/packages/tools/utilities/autorefs-109670
// Refactored by: enginooby

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Enginooby.Attribute;
using Enginooby.Utils;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

// TODO: This is an Editor script, what about in build case?
// Use menu tool to set AutoRefs for SerializeField before build
public class AutoRefs : Editor {
  // which contains [AutoRef]
  private static MonoBehaviour _currentMonoBehaviour;

  // which annotated with [AutoRef]
  private static FieldInfo _currentFieldInfo;

  // all targets referenced by current MonoBehaviour
  private static readonly List<GameObject> AutoRefGameObjects = new();

  [PostProcessScene] // ?
  private static void OnPostProcessScene() => SetAutoRefs();

  [MenuItem("Tools/AutoRefs/Set AutoRefs")]
  private static void SetAutoRefs() {
    var gos = FindObjectsOfType<GameObject>();
    Undo.SetCurrentGroupName("Undo AutoRefs");
    var undoGroup = Undo.GetCurrentGroup();
    gos?.ForEach(ProcessGameObject);
    Undo.CollapseUndoOperations(undoGroup);
  }

  private static void ProcessGameObject(GameObject go) {
    var monoBehaviours = go.GetComponents<MonoBehaviour>().ToList<MonoBehaviour>();
    monoBehaviours.RemoveNullEntries();
    Undo.RecordObjects(monoBehaviours.ToArray(), "AutoRefs MonoBehaviours");
    monoBehaviours.ForEach(ProcessMonoBehaviour);
  }

  private static void ProcessMonoBehaviour(MonoBehaviour monoBehaviour) {
    _currentMonoBehaviour = monoBehaviour;
    var type = _currentMonoBehaviour.GetType();
    var fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    fieldInfos.ForEach(ProcessFieldInfo);
  }

  private static void ProcessFieldInfo(FieldInfo fieldInfo) {
    // Check that field for the AutoRef attribute.
    _currentFieldInfo = fieldInfo;
    var autoRefAttribute = (AutoRefAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(AutoRefAttribute));
    if (autoRefAttribute is null) return;
    GetAutoRefGameObjects(autoRefAttribute);
    ProcessAutoRefAttribute();
  }

  private static void ProcessAutoRefAttribute() {
    if (AutoRefGameObjects.IsNullOrEmpty()) {
      Debug.LogError(
        "AutoRefs: No targets were found for AutoRef. Please ensure the [AutoRef] attribute applied includes a valid target type.\nMonoBehaviour: [" +
        _currentMonoBehaviour.name + "]	Field: [" + _currentFieldInfo.Name + "]");
      return;
    }

    var fieldType = _currentFieldInfo.FieldType;
    try {
      if (fieldType.IsArray)
        ProcessFieldArrayType();
      else if (fieldType.IsGenericType)
        ProcessFieldGenericType();
      // Find a component with a matching type to the field.
      else if (AutoRefGameObjects[0].TryGetComponent(fieldType, out var component))
        // Set the value of that field on the monobehaviour to match the found component.
        // This is where the reference is set.
        _currentFieldInfo.SetValue(_currentMonoBehaviour, component);
      else
        LogErrorAutoRefFailedToFindReference();
    }
    catch (ArgumentException) {
      Debug.LogError(
        "AutoRefs: An incompatible type has been annotated with the [AutoRef].\nType: [" + fieldType.Name +
        "]	MonoBehaviour: [" + _currentMonoBehaviour.name + "]	Field: [" + _currentFieldInfo.Name + "]");
    }
  }

  // UTIL
  private static void ProcessFieldGenericType() {
    if (!_currentFieldInfo.FieldType.IsListType()) return;

    // Get the element type of the list (only 1 argument for a list, so always [0]).
    var elementType = _currentFieldInfo.FieldType.GetGenericArguments()[0];
    var components = new List<Component>();

    foreach (var autoRefGo in AutoRefGameObjects) components.AddRange(autoRefGo.GetComponents(elementType));

    if (components.IsNullOrEmpty()) {
      LogErrorAutoRefFailedToFindReference();
      return;
    }

    // Find all components which match that element type (may be multiple components).
    var componentArray = components.ToArray();
    // Get the type of a list of the field element type.
    var genericElementType = typeof(List<>).MakeGenericType(elementType);
    // Create an IList of the correct type.
    var cIList = Activator.CreateInstance(genericElementType) as IList;
    // For each component which was found, copy it into the IList.
    foreach (var component in componentArray) cIList?.Add(component);
    // Set the value of the FieldInfo to the value of the IList, which fills the original list.
    _currentFieldInfo.SetValue(_currentMonoBehaviour, cIList);
  }

  // UTIL
  private static void ProcessFieldArrayType() {
    // Get the element type of the array (i.e. int[] the element type is int).
    var fieldType = _currentFieldInfo.FieldType;
    var elementType = fieldType.GetElementType();
    var components = new List<Component>();

    foreach (var autoRefGameObject in AutoRefGameObjects)
      components.AddRange(autoRefGameObject.GetComponents(elementType));

    if (components.IsNullOrEmpty()) {
      LogErrorAutoRefFailedToFindReference();
      return;
    }

    // Find all components which match that element type (may be multiple components).
    var componentArray = components.ToArray();
    // Create an array of the element type with the same length as the array of components which were found.
    // This is necessary to convert the Component[] type array into an array of the type specified in the FieldInfo.
    // i.e. The FieldInfo may be an array MyClass[]. You cannot set the value of a MyClass[] to a Component[].
    var componentObjects = Array.CreateInstance(elementType, componentArray.Length);
    // For each component which was found, copy it into the correctly typed array.
    for (var i = 0; i < componentArray.Length; i++) componentObjects.SetValue(componentArray[i], i);

    // Set the value of the FieldInfo to the newly created array.
    _currentFieldInfo.SetValue(_currentMonoBehaviour, componentObjects);
  }

  private static void LogErrorAutoRefFailedToFindReference() {
    Debug.LogError(
      "AutoRefs: AutoRef failed to find a matching reference. Please ensure the [AutoRef] attribute applied" +
      " includes a valid target type.\nType: [" + _currentFieldInfo.FieldType.Name + "]	" +
      "MonoBehaviour: [" + _currentMonoBehaviour.name + "]	Field: [" + _currentFieldInfo.Name + "]");
  }

  private static void GetAutoRefGameObjects(AutoRefAttribute autoRefAttribute) {
    var autoRefTarget = autoRefAttribute.Target;
    var go = _currentMonoBehaviour.gameObject;
    AutoRefGameObjects.Clear();

    // if ((targetType & AutoRefTargetType.Self) != AutoRefTargetType.Undefined) {} // ? <=> Has flag
    if (autoRefTarget.HasFlag(AutoRefTarget.Self))
      AutoRefGameObjects.Add(go);

    if (autoRefTarget.HasFlag(AutoRefTarget.Parent))
      AutoRefGameObjects.Add(go.transform.parent.gameObject);

    if (autoRefTarget.HasFlag(AutoRefTarget.Children))
      AutoRefGameObjects.AddRange(go.GetChildGameObjects());

    if (autoRefTarget.HasFlag(AutoRefTarget.Siblings))
      AutoRefGameObjects.AddRange(go.GetSiblingGameObjects());

    if (autoRefTarget.HasFlag(AutoRefTarget.Scene))
      AutoRefGameObjects.AddRange(FindObjectsOfType<GameObject>());

    if (autoRefTarget.HasFlag(AutoRefTarget.NamedGameObjects)) {
      if (autoRefAttribute.GameObjectNames.IsNullOrEmpty()) {
        Debug.LogError(
          "AutoRefs: The target type of NamedGameObjects requires " +
          "the names of the GameObjects to be set in the AutoRefs attribute.");
        return;
      }

      foreach (var name in autoRefAttribute.GameObjectNames)
        AutoRefGameObjects.AddRange(GameObjectUtils.FindAllGameObjects(name));
    }
  }
}