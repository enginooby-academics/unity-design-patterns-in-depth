using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

namespace IteratorPattern.Case1.Base {
  public class IterablePathList : MonoBehaviour, IIterable<Waypoint> {
    [SerializeField] [InlineEditor] private List<Waypoint> _waypoints = new List<Waypoint>();

    // ? Alternative: implement indexer and pass this iterable to iterator's constructor
    // IMPL: multiple iterator strategies
    public IIterator<Waypoint> GetIterator() =>
      // return new Iterator(_waypoints);
      // return new IteratorFilterColor(_waypoints);
      new IteratorFilterSizeMin(_waypoints);

    [Button]
    public void AddAllChildren() {
      _waypoints = transform.GetComponentsInChildren<Waypoint>().ToList();
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