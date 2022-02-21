using System;
using UnityEngine;
using Random = UnityEngine.Random;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace CompositePattern.Case2.Unity1 {
  /// <summary>
  ///   * The 'Leaf' base class
  /// </summary>
  [Serializable]
  [InlineProperty]
  public abstract class Shape : MonoBehaviourGizmos, IShape {
    protected float _scale;

    protected virtual void Start() {
      _scale = Random.Range(.5f, 1.5f);
      gameObject.SetScale(_scale);
      gameObject.AddComponent<MeshFilter>();
      gameObject.AddComponent<MeshRenderer>();
      gameObject.AddComponent<BoxCollider>();
      gameObject.SetMaterialColor(Color.green);
    }

    private void OnMouseDown() => GetVolume().Log();

    public abstract double GetVolume();
  }
}