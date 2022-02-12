using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AdapterPattern.Case1.Base1 {
  /// <summary>
  ///   * [The 'Adapter' base class]
  /// </summary>
  public abstract class ColorizableObject<T, W> : IColorizable where T : Object where W : Component {
    protected W _component;

    protected T _object;

    public ColorizableObject() {
    }

    public ColorizableObject(W component) {
      _component = component;
      if (typeof(T) == typeof(W)) _object = component as T;
    }

    public abstract Color Color { get; set; }
    public Type ComponentType => typeof(W);
  }

  public abstract class ColorizableObject<T> : ColorizableObject<T, T> where T : Component {
    public ColorizableObject() {
    }

    protected ColorizableObject(T component) : base(component) {
    }
  }
}