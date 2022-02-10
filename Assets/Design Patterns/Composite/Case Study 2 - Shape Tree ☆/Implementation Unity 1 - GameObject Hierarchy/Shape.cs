using System;
using Drawing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CompositePattern.Case2.Unity1 {
  /// <summary>
  /// * The 'Leaf' base class
  /// </summary>
  [Serializable, InlineProperty]
  public abstract class Shape : MonoBehaviourGizmos, IShape {
    protected float _scale;

    protected virtual void Start() {
      _scale = UnityEngine.Random.Range(.5f, 1.5f);
      gameObject.SetScale(_scale);
      gameObject.AddComponent<MeshFilter>();
      gameObject.AddComponent<MeshRenderer>();
      gameObject.AddComponent<BoxCollider>();
      gameObject.SetMaterialColor(Color.green);
    }

    public abstract double GetVolume();

    private void OnMouseDown() => GetVolume().Log();
  }
}
