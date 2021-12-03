using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IteratorPattern.Case1.Base {
  // TIP: Extend OdinInspector.SerializedMonoBehaviour to serialize queue/stack...
  public class IterablePathQueue : SerializedMonoBehaviour, IIterable<Waypoint> {
    [SerializeField, InlineEditor]
    private Queue<Waypoint> _waypoints = new Queue<Waypoint>();

    [Button]
    public void AddAllChildren() {
      _waypoints = new Queue<Waypoint>(transform.GetComponentsInChildren<Waypoint>());
    }

    public IIterator<Waypoint> GetIterator() {
      return new IteratorFilterSizeMin(_waypoints.ToList());
    }
  }
}
