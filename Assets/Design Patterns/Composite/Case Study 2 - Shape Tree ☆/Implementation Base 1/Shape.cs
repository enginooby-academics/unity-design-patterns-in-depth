using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CompositePattern.Case2.Base1 {
  /// <summary>
  /// * The 'Leaf' base class
  /// </summary>
  [Serializable, InlineProperty]
  public abstract class Shape : IShape {
    protected float _scale;

    private GameObject _gameObject;

    public GameObject GameObject { get => _gameObject; set => _gameObject = value; }

    protected Shape(PrimitiveType type) {
      if (!Application.isPlaying) return;

      _scale = UnityEngine.Random.Range(.5f, 2f);
      _gameObject = GameObject.CreatePrimitive(type);
      _gameObject.SetScale(_scale);
    }

    public abstract double GetVolume();

    [Button]
    public void LogVolume() => GetVolume().Log();
  }
}
