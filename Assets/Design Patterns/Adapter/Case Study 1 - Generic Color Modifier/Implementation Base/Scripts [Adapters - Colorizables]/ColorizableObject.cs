using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AdapterPattern.Case1.Base1 {
  /// <summary>
  ///   * The 'Adapter' base class
  /// </summary>
  public abstract class ColorizableObject<TObject, TComponent> : IColorizable
    where TObject : Object where TComponent : Component {
    protected readonly TComponent _component;
    protected TObject _object;

    public ColorizableObject() { }

    public ColorizableObject(TComponent component) {
      _component = component;
      if (typeof(TObject) == typeof(TComponent)) _object = component as TObject;
    }

    public abstract Color Color { get; set; }

    public Type ComponentType => typeof(TComponent);
  }

  public abstract class ColorizableObject<TComponent> : ColorizableObject<TComponent, TComponent>
    where TComponent : Component {
    public ColorizableObject() { }

    protected ColorizableObject(TComponent component) : base(component) { }
  }
}