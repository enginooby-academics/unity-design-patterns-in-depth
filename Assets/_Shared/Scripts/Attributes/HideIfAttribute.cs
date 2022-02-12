using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
  [Conditional("UNITY_EDITOR")]
  public class HideIfAttribute : PropertyAttribute {
    private string _condition;
    private object _optionalValue;
    public bool Animate;

    public HideIfAttribute(string condition, object optionalValue) {
      _condition = condition;
      _optionalValue = optionalValue;
    }

    public HideIfAttribute(string condition) => _condition = condition;
  }
}