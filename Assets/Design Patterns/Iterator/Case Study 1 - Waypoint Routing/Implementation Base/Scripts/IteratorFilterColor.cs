using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

// ? Implement Decorator for multiple filter combinations
// ? Extend from Iterator base
namespace IteratorPattern.Case1.Base {
  /// <summary>
  /// Iterator loops through all waypoints having match color.
  /// </summary>
  [System.Serializable, InlineProperty]
  public class IteratorFilterColor : IIterator<Waypoint> {
    [SerializeField, EnumToggleButtons, OnValueChanged(nameof(UpdateFilteredWaypoints))]
    private Waypoint.Color _filterColor = Waypoint.Color.Green;
    private int _current = 0;
    private List<Waypoint> _waypoints;

    [ShowInInspector]
    private List<Waypoint> _filteredWaypoints;

    public List<Waypoint> FilteredWaypoints => _filteredWaypoints;

    public Waypoint.Color FilterColor {
      get => _filterColor;
      set {
        _filterColor = value;
        UpdateFilteredWaypoints();
      }
    }

    private void UpdateFilteredWaypoints() {
      _filteredWaypoints = _waypoints.Where(waypoint => waypoint.GetColor() == _filterColor).ToList();
    }

    public IteratorFilterColor(List<Waypoint> waypoints) {
      _waypoints = waypoints;
      UpdateFilteredWaypoints();
    }

    public Waypoint GetNext() {
      return _filteredWaypoints[_current++];
    }

    public bool HasNext() {
      return _current < _filteredWaypoints.Count;
    }
  }
}