using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IteratorPattern.Case1.Base {
  public class IterablePathStack : SerializedMonoBehaviour, IIterable<Waypoint> {
    [SerializeField, InlineEditor]
    private Stack<Waypoint> _waypoints = new Stack<Waypoint>();

    [Button]
    public void AddAllChildren() {
      _waypoints = new Stack<Waypoint>(transform.GetComponentsInChildren<Waypoint>());
    }

    public IIterator<Waypoint> GetIterator() {
      return new IteratorFilterSizeMin(_waypoints.ToList());
    }
  }
}
