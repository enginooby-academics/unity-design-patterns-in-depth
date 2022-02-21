using System;
using UnityEngine;
using Random = UnityEngine.Random;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace CompositePattern.Case2.Base1 {
  /// <summary>
  ///   * The 'Leaf' base class
  /// </summary>
  [Serializable]
  [InlineProperty]
  public abstract class Shape : IShape {
    private GameObject _gameObject;
    protected float _scale;

    protected Shape(PrimitiveType type) {
      if (!Application.isPlaying) return;

      _scale = Random.Range(.5f, 2f);
      _gameObject = GameObject.CreatePrimitive(type);
      _gameObject.SetScale(_scale);
      _gameObject.SetMaterialColor(Color.green);
    }

    public GameObject GameObject {
      get => _gameObject;
      set => _gameObject = value;
    }

    public abstract double GetVolume();

    [Button]
    public void LogVolume() => GetVolume().Log();
  }
}