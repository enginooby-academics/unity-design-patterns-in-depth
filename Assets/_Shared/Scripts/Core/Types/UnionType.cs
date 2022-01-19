using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable, InlineProperty]
public class UnionType<T1, T2> {
  [SerializeField, ValueDropdown(nameof(Types)), HideLabel]
  private string _currentType;

  [ShowIf(nameof(IsT1))]
  [SerializeField, LabelText("Value")]
  private T1 _valueT1;

  [ShowIf(nameof(IsT2))]
  [SerializeField, LabelText("Value")]
  private T2 _valueT2;

  public Type Type => IsT1 ? typeof(T1) : typeof(T2);

  public dynamic Value {
    get {
      if (IsT1) return _valueT1;
      if (IsT2) return _valueT2;
      return null;
    }
  }

  private bool IsT1 => _currentType.Equals(typeof(T1).Name);
  private bool IsT2 => _currentType.Equals(typeof(T2).Name);

  private IEnumerable Types = new ValueDropdownList<string>()
{
    { typeof(T1).Name, typeof(T1).Name},
    { typeof(T2).Name, typeof(T2).Name},
};

  private List<string> TypeNames => new List<string> { typeof(T1).Name, typeof(T2).Name };
}


[Serializable, InlineProperty]
public class UnionStructType<T1, T2> : UnionType<T1, T2>
where T1 : struct
where T2 : struct { }


[Serializable, InlineProperty]
public class VectorType : UnionType<Vector2, Vector3> { }