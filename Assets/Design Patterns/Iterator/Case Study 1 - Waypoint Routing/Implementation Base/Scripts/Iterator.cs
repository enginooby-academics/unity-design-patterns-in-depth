using System.Collections.Generic;

namespace IteratorPattern.Case1.Base {
  /// <summary>
  /// Normal base iterator which loops through all waypoints.
  /// </summary>
  public class Iterator : IIterator<Waypoint> {
    // TODO: travesal variations - step, direction, random, loop, ping-pong
    private int _current = 0;
    private List<Waypoint> _waypoints;

    public Iterator(List<Waypoint> waypoints) {
      _waypoints = waypoints;
    }

    public Waypoint GetNext() {
      return _waypoints[_current++];
    }

    public bool HasNext() {
      return _current < _waypoints.Count;
    }
  }
}