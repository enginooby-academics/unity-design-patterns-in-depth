#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
using Enginoobz.Core;
#endif

using System.Linq;
using System.Collections.Generic;
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
