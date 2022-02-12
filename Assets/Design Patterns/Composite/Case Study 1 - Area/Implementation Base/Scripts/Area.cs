using System;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

// TODO: Exclusive area
namespace CompositePattern.Case1.Base {
  public enum GizmosMode {
    Solid,
    Wire
  }

  [Flags]
  public enum GizmosDisplay {
    OnGizmos = 1 << 1,
    OnSelected = 1 << 2,
    InGame = 1 << 3
  }

  [Serializable]
  [InlineProperty]
  /// <summary>
  /// The 'Component' Treenode
  /// * Define area for detection, boundary, trigger, spawner, etc.
  /// </summary>
  public abstract class Area {
    [HideIf(nameof(_isComposite))]
    [ToggleGroup(nameof(_isEnabled), "Enable")]
    [SerializeField]
    [ToggleLeft]
    [GUIColor(nameof(_gizmosColor))]
    protected bool _isEnabled = true;

    [ToggleGroup(nameof(_isEnabled))] [HideIf(nameof(_isComposite))] [LabelText("Area Origins")] [SerializeField]
    protected List<ReferenceVector3> _origins = new List<ReferenceVector3>();

    [ToggleGroup(nameof(_isEnabled))]
    [FoldoutGroup(nameof(_isEnabled) + "/Gizmos")]
    // [FoldoutGroup("Gizmos")]
    [HideIf(nameof(_isComposite))]
    [SerializeField]
    protected Color _gizmosColor = Color.cyan;

    [ToggleGroup(nameof(_isEnabled))]
    [FoldoutGroup(nameof(_isEnabled) + "/Gizmos")]
    // [FoldoutGroup("Gizmos")]
    [HideIf(nameof(_isComposite))]
    [SerializeField]
    [Range(.5f, 10f)]
    protected float _gizmosWidth = 1.5f;

    [ToggleGroup(nameof(_isEnabled))]
    // [FoldoutGroup("Gizmos")]
    [FoldoutGroup(nameof(_isEnabled) + "/Gizmos")]
    [HideIf(nameof(_isComposite))]
    [SerializeField]
    [EnumToggleButtons]
    [HideLabel]
    protected GizmosMode _gizmosMode;

    [ToggleGroup(nameof(_isEnabled))]
    // [FoldoutGroup("Gizmos")]
    [FoldoutGroup(nameof(_isEnabled) + "/Gizmos")]
    [HideIf(nameof(_isComposite))]
    [SerializeField]
    [EnumToggleButtons]
    [HideLabel]
    protected GizmosDisplay _gizmosDisplay = GizmosDisplay.InGame;

    protected bool _isComposite;

    public Area() {
    }

    public Area(Vector3 staticOrigin) {
      _origins.Add(new ReferenceVector3(staticOrigin));
    }

    public Area(GameObject gameObjectOrigin) {
      _origins.Add(new ReferenceVector3(gameObjectOrigin));
    }

    public bool IsEnabled => _isEnabled;

    /// <summary>
    ///   Return a random position lie inside the area.
    /// </summary>
    public abstract Vector3 RandomPoint { get; }

    public abstract bool Contains(Vector3 pos);

    public bool Contains(GameObject target) => Contains(target.transform.position);

    public bool Contains(Reference reference) => Contains(reference.GameObject);

    /// <summary>
    ///   Visualize area in MonoBehaviourGizmos.DrawGizmos().
    /// </summary>
    public virtual void DrawGizmos(Color? color = null) {
      if (!_isEnabled) return;

      if (color.HasValue) _gizmosColor = color.Value;
      _origins.ForEach(origin => {
        if (origin.HasValue) DrawGizmosOnSingleOrigin(origin);
      });
    }

    protected abstract void DrawGizmosOnSingleOrigin(ReferenceVector3 origin);
  }
}