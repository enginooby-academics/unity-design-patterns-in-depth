using System.Collections.Generic;
using System.Linq;

namespace IteratorPattern.Case1.Base {
  /// <summary>
  ///   Iterator loops through all waypoints having size greater then filter min size (of the iterator).
  /// </summary>
  public class IteratorFilterSizeMin : IIterator<Waypoint> {
    private int _current;
    private float _filterMinSize;
    private readonly List<Waypoint> _waypoints;

    public IteratorFilterSizeMin(List<Waypoint> waypoints, float filterMinSize = 1f) {
      _waypoints = waypoints.Where(waypoint => waypoint.GetSize() >= filterMinSize).ToList();
      _filterMinSize = filterMinSize;
    }

    public Waypoint GetNext() => _waypoints[_current++];

    public bool HasNext() => _current < _waypoints.Count;
  }
}