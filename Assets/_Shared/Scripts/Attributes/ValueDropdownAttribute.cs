using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginooby.Attribute {
#if ODIN_INSPECTOR
  // ! Not working
  // /// <inheritdoc />
  // public class ValueDropdownAttribute : Sirenix.OdinInspector.ValueDropdownAttribute {
  //   public ValueDropdownAttribute(string valuesGetter) : base(valuesGetter) { }
  // }
#else
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class ValueDropdownAttribute : PropertyAttribute {
    private string _text;

    public ValueDropdownAttribute(string text) => _text = text;
  }
#endif
}