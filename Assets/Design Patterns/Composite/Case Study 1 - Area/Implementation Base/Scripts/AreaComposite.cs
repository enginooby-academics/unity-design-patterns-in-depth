using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace CompositePattern.Case1.Base {
  [Serializable]
  [InlineProperty]
  /// <summary>
  /// * The 'Composite' class.
  /// </summary>
  public class AreaComposite : Area {
    [SerializeField] [SerializeReference] private List<Area> _areas = new();


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

    public override Vector3 RandomPoint => EnableAreas.GetRandom().RandomPoint; // ? Performance

    public virtual AreaComposite Add(Area area) {
      _areas.Add(area);
      return this;
    }

    public virtual void Remove(Area area) {
      _areas.Remove(area);
    }

    public override bool Contains(Vector3 pos) {
      if (!_isEnabled) return false;

      foreach (var area in _areas)
        if (area.Contains(pos))
          return true;

      return false;
    }

    public override void DrawGizmos(Color? color = null) {
      if (_areas.IsNullOrEmpty()) return;

      foreach (var area in _areas) area?.DrawGizmos(color);
    }

    protected override void DrawGizmosOnSingleOrigin(ReferenceVector3 origin) { }
  }
}