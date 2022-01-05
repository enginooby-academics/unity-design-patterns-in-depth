using System.Linq;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CompositePattern.Case1.Base {
  [Serializable, InlineProperty]
  /// <summary>
  /// * The 'Composite' class.
  /// </summary>
  public class AreaComposite : Area {
    [SerializeField, SerializeReference]
    private List<Area> _areas = new List<Area>();


    public AreaComposite(List<Area> areas) {
      _areas = areas;
      _isComposite = true;
    }

    public AreaComposite() {
      _areas = new List<Area>();
      _isComposite = true;
    }

    public List<Area> Areas => _areas;
    public List<Area> EnableAreas => Areas.Where(area => area.IsEnabled).ToList();

    public virtual AreaComposite Add(Area area) {
      _areas.Add(area);
      return this;
    }

    public virtual void Remove(Area area) {
      _areas.Remove(area);
    }

    public override Vector3 RandomPoint {
      get {
        return EnableAreas.GetRandom().RandomPoint; // ? Performance
      }
    }

    public override bool Contains(Vector3 pos) {
      if (!_isEnabled) return false;

      foreach (var area in _areas) {
        if (area.Contains(pos)) return true;
      }

      return false;
    }

    public override void DrawGizmos(Color? color = null) {
      if (_areas.IsUnset()) return;

      foreach (var area in _areas) {
        area?.DrawGizmos(color);
      }
    }

    protected override void DrawGizmosOnSingleOrigin(ReferenceVector3 origin) {
    }
  }
}