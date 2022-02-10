using System;
using Drawing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CompositePattern.Case2.Unity3 {
  /// <summary>
  /// * The 'Leaf' base class
  /// </summary>
  [Serializable, InlineProperty]
  public abstract class Shape : MonoBehaviourGizmos, IShape {
    [SerializeField, Range(.5f, 2f), OnValueChanged(nameof(UpdateScale))]
    protected float _scale = 1f;

    private void UpdateScale() => gameObject.SetScale(_scale);

    public abstract double GetVolume();

    private void OnMouseDown() => GetVolume().Log();
  }
}
