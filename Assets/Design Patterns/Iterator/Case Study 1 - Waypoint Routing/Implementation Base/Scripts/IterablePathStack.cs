using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
using Enginooby.Core;
#endif

namespace IteratorPattern.Case1.Base {
  public class IterablePathStack : SerializedMonoBehaviour, IIterable<Waypoint> {
    [SerializeField] private Stack<Waypoint> _waypoints = new();

    public IIterator<Waypoint> GetIterator() => new IteratorFilterSizeMin(_waypoints.ToList());

    [Button]
    public void AddAllChildren() {
      _waypoints = new Stack<Waypoint>(transform.GetComponentsInChildren<Waypoint>());
    }
  }
}