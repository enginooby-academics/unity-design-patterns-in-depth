using System;
using UnityEngine;

namespace AdapterPattern.Case1.Base1 {
  /// <summary>
  /// * [The 'Adapter' base class]
  /// </summary>
  public abstract class ColorizableObject<T, W> : IColorizable where T : UnityEngine.Object where W : Component {
    public abstract Color Color { get; set; }
    public Type ComponentType => typeof(W);

    protected T _object;
    protected W _component;

    public ColorizableObject() { }

    public ColorizableObject(W component) {
      _component = component;
      if (typeof(T) == typeof(W)) _object = component as T;
    }
  }

  public abstract class ColorizableObject<T> : ColorizableObject<T, T> where T : Component {
    public ColorizableObject() { }
    protected ColorizableObject(T component) : base(component) { }
  }
}