using System.Collections.Generic;
using System.Linq;

namespace IteratorPattern.Case1.Base {
  /// <summary>
  /// Iterator loops through all waypoints having size greater then filter min size (of the iterator).
  /// </summary>
  public class IteratorFilterSizeMin : IIterator<Waypoint> {
    private float _filterMinSize;
    private int _current = 0;
    private List<Waypoint> _waypoints;

    public IteratorFilterSizeMin(List<Waypoint> waypoints, float filterMinSize = 1f) {
      _waypoints = waypoints.Where(waypoint => waypoint.GetSize() >= filterMinSize).ToList();
      _filterMinSize = filterMinSize;
    }

    public Waypoint GetNext() {
      return _waypoints[_current++];
    }

    public bool HasNext() {
      return _current < _waypoints.Count;
    }
  }
}