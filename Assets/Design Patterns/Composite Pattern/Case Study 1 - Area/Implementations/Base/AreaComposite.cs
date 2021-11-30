using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CompositePattern.Case1.Base {
  [Serializable, InlineProperty]
  /// <summary>
  /// 'Composite' class.
  /// </summary>
  public class AreaComposite : Area {
    [SerializeField, SerializeReference]
    private List<Area> _areas = new List<Area>();


    public AreaComposite(List<Area> areas) {
      _areas = areas;
      _isAreaComposite = true;
    }

    public AreaComposite() {
      _areas = new List<Area>();
      _isAreaComposite = true;
    }

    public List<Area> Areas => _areas;

    public virtual void Add(Area area) {
      _areas.Add(area);
    }

    public virtual void Remove(Area area) {
      _areas.Remove(area);
    }

    public override Vector3 RandomPoint {
      get {
        return _areas.GetRandom().RandomPoint;
      }
    }

    public override bool Contains(Vector3 pos) {
      foreach (var area in _areas) {
        if (area.Contains(pos)) return true;
      }

      return false;
    }

    public override void DrawGizmos(Color? color = null) {
      if (_areas.IsUnset()) return;

      foreach (var area in _areas) {
        area.DrawGizmos(color);
      }
    }

    protected override void DrawGizmosOnSingleOrigin(ReferenceVector3 origin) {
    }
  }
}