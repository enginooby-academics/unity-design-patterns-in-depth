using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IteratorPattern.Case1.Base {
  public class IterablePathList : MonoBehaviour, IIterable<Waypoint> {
    [SerializeField, InlineEditor]
    private List<Waypoint> _waypoints = new List<Waypoint>();

    [Button]
    public void AddAllChildren() {
      _waypoints = transform.GetComponentsInChildren<Waypoint>().ToList();
    }

    // ? Alternative: implement indexer and pass this iterable to iterator's constructor
    // IMPL: multiple iterator strategies
    public IIterator<Waypoint> GetIterator() {
      // return new Iterator(_waypoints);
      // return new IteratorFilterColor(_waypoints);
      return new IteratorFilterSizeMin(_waypoints);
    }

    // IMPL: Reflection w/ generic type
    // public T GetIterator<T>() where T : IIterator<Waypoint> {
    //   return (T)System.Activator.CreateInstance(typeof(T), _waypoints);
    // }

    // public IIterator<Waypoint> GetIterator(Type iteratorType) {
    //   return (IIterator<Waypoint>)System.Activator.CreateInstance(iteratorType, _waypoints);
    // }
  }
}
