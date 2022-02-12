using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

namespace CompositePattern.Case2.Unity2 {
  /// <summary>
  ///   * The 'Leaf' base class
  /// </summary>
  [Serializable]
  [InlineProperty]
  public abstract class Shape : MonoBehaviourGizmos, IShape {
    [SerializeField] [Range(.5f, 2f)] [OnValueChanged(nameof(UpdateScale))]
    protected float _scale = 1f;

    private void OnMouseDown() => GetVolume().Log();

    public abstract double GetVolume();

    private void UpdateScale() => gameObject.SetScale(_scale);
  }
}