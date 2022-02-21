using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// ? Implement Decorator for multiple filter combinations
// ? Extend from Iterator base
namespace IteratorPattern.Case1.Base {
  /// <summary>
  ///   Iterator loops through all waypoints having match color.
  /// </summary>
  [Serializable]
  [InlineProperty]
  public class IteratorFilterColor : IIterator<Waypoint> {
    [SerializeField] [EnumToggleButtons] [OnValueChanged(nameof(UpdateFilteredWaypoints))]
    private Waypoint.Color _filterColor = Waypoint.Color.Green;

    private int _current;

    [ShowInInspector] private List<Waypoint> _filteredWaypoints;

    private List<Waypoint> _waypoints;

    public IteratorFilterColor(List<Waypoint> waypoints) {
      _waypoints = waypoints;
      UpdateFilteredWaypoints();
    }

    public List<Waypoint> FilteredWaypoints => _filteredWaypoints;

    public Waypoint.Color FilterColor {
      get => _filterColor;
      set {
        _filterColor = value;
        UpdateFilteredWaypoints();
      }
    }

    public Waypoint GetNext() => _filteredWaypoints[_current++];

    public bool HasNext() => _current < _filteredWaypoints.Count;

    private void UpdateFilteredWaypoints() {
      _filteredWaypoints = _waypoints.Where(waypoint => waypoint.GetColor() == _filterColor).ToList();
    }
  }
}