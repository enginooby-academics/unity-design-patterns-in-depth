using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

// TODO: Exclusive area
namespace CompositePattern.Case1.Base {
  public enum GizmosMode { Solid, Wire }

  [Serializable, InlineProperty]
  /// <summary>
  /// 'Leaf' class.
  /// </summary>
  public abstract class Area {
    public Area() {
    }

    public Area(Vector3 staticOrigin) {
      _origins.Add(new ReferenceVector3(staticOrigin));
    }

    public Area(GameObject gameObjectOrigin) {
      _origins.Add(new ReferenceVector3(gameObjectOrigin));
    }

    protected bool _isAreaComposite;

    [HideIf(nameof(_isAreaComposite))]
    [LabelText("Area Origins")]
    [SerializeField]
    protected List<ReferenceVector3> _origins = new List<ReferenceVector3>();

    [HideIf(nameof(_isAreaComposite))]
    [SerializeField]
    protected Color _gizmosColor = Color.cyan;

    [HideIf(nameof(_isAreaComposite))]
    [SerializeField, EnumToggleButtons]
    protected GizmosMode _gizmosMode;

    public abstract bool Contains(Vector3 pos);

    public bool Contains(GameObject target) {
      return Contains(target.transform.position);
    }

    public bool Contains(Reference reference) {
      return Contains(reference.GameObject);
    }

    /// <summary>
    /// Return a random position lie inside the area.
    /// </summary>
    public abstract Vector3 RandomPoint { get; }

    public virtual void DrawGizmos(Color? color = null) {
      if (color.HasValue) _gizmosColor = color.Value;
      _origins.ForEach(DrawGizmosOnSingleOrigin);
    }

    protected abstract void DrawGizmosOnSingleOrigin(ReferenceVector3 origin);
  }
}