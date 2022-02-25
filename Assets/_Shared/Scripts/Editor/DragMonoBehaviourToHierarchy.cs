using System.Collections.Generic;
using Enginooby.Utils;
using UnityEditor;
using UnityEngine;

/// <summary>
///   Utility: drag MonoBehaviour script to Hierarchy window to create new GameObject.
/// </summary>
[InitializeOnLoad] // makes sure that the static constructor is always called in the editor. 
public class DragMonoBehaviourToHierarchy : Editor {
  private static GameObject[] _selectedGameObjects;

  static DragMonoBehaviourToHierarchy() {
    // Adds a callback for when the hierarchy window processes GUI events
    // for every GameObject in the hierarchy.
    EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemCallback;
    EditorApplication.hierarchyWindowChanged += HierarchyWindowChanged;
  }

  // TIP: Add de-constructor for editor script to unsubscribe events
  ~DragMonoBehaviourToHierarchy() {
    EditorApplication.hierarchyWindowItemOnGUI -= HierarchyWindowItemCallback;
    EditorApplication.hierarchyWindowChanged -= HierarchyWindowChanged;
  }

  private static void HierarchyWindowChanged() {
    if (!_selectedGameObjects.IsNullOrEmpty())
      Selection.objects = _selectedGameObjects;
  }

  private static void HierarchyWindowItemCallback(int id, Rect rect) {
    if (Event.current.type != EventType.DragExited) return;
    // happens when an acceptable item is released over the GUI window
    // get all the drag and drop information ready for processing.
    DragAndDrop.AcceptDrag();

    // used to emulate selection of new objects.
    var selectedObjects = new List<GameObject>();
    ProcessDraggedAssets(selectedObjects);
    // we didn't drag any assets, so do nothing.
    if (selectedObjects.Count == 0) return;
    // emulate selection of newly created objects.
    _selectedGameObjects = selectedObjects.ToArray();
    // make sure this call is the only one that processes the event.
    Event.current.Use();
  }

  private static void ProcessDraggedAssets(ICollection<GameObject> selectedObjects) {
    foreach (var asset in DragAndDrop.objectReferences) {
      if (asset.GetType() != typeof(MonoScript)) break;
      // only process script assets
      var scriptName = asset.name;
      var type = TypeUtils.GetType(scriptName);

      if (type is null || !type.Is<MonoBehaviour>()) continue;
      // only process MonoBehaviour scripts
      var mono = asset as Component;
      var go = new GameObject(asset.name.AddSpacesBeforeCapitals());
      var component = go.AddComponent(type);
      go.transform.Reset();

      selectedObjects.Add(go);
    }
  }
}