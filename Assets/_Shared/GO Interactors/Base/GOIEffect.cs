using System;
using Sirenix.OdinInspector;
using UnityEngine;

// ? Convert to SO

[Serializable, InlineProperty]
public abstract class GOIEffect {
  public abstract void Increment();
  public abstract void Decrement();
}

[Serializable, InlineProperty]
public abstract class GOIEffect<T> : GOIEffect {
  [SerializeField]
  protected T _value;

  public T Value => _value;

  public GOIEffect(T value) {
    _value = value;
  }
}


// [Serializable, InlineProperty]
// public class GOInteractEffectEnum<T> : GOInteractEffect<T> where T : struct {
//   public GOInteractEffectEnum(T value) : base(value) {
//   }

//   public override void Increment() => _value = _value.Next();
//   public override void Decrement() => _value = _value.Previous();
// }