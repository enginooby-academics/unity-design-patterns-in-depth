using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginooby.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class ValueDropdownAttribute : PropertyAttribute {
    private string _text;

    public ValueDropdownAttribute(string text) => _text = text;
  }
}