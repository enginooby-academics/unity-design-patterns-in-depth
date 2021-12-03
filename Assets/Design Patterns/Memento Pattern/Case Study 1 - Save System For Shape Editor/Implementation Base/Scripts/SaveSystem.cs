using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// IMPL: UI
namespace MementoPattern.Case1.Base {
  /// <summary>
  /// The 'Caretaker' class. Responsible for managing & loading snapshots.
  /// </summary>
  public class SaveSystem : MonoBehaviourSingleton<SaveSystem> {
    [SerializeField]
    private Shape _currentShape;

    [ShowInInspector]
    private List<ShapeSnapshot> _history = new List<ShapeSnapshot>();

    public void AddSnapshot(ShapeSnapshot snapshot) {
      _history.Add(snapshot);
    }

    public void LoadSnapshot(ShapeSnapshot snapshot) {
      _currentShape.LoadSnapshot(snapshot);
      print($"Snapshot {snapshot.Name} loaded.");
    }

    public bool CheckSnapshotExist(string snapshotName) {
      ShapeSnapshot snapshot = _history.Find(snapshot => snapshot.Name.EqualIgnoreCase(snapshotName));
      return snapshot == null ? false : true;
    }

    // [Button]
    public void LoadSnapshot(string snapshotName) {
      ShapeSnapshot snapshot = _history.Find(snapshot => snapshot.Name.EqualIgnoreCase(snapshotName));
      if (snapshot != null) LoadSnapshot(snapshot);
      else Debug.LogError($"Snapshot {snapshotName} can not be found.");
    }
  }
}
