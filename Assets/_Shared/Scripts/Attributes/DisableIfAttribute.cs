using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
  [Conditional("UNITY_EDITOR")]
  public class DisableIfAttribute : PropertyAttribute {
    private string _condition;
    private object _optionalValue;

    public DisableIfAttribute(string condition, object optionalValue) {
      _condition = condition;
      _optionalValue = optionalValue;
    }

    public DisableIfAttribute(string condition) => _condition = condition;
  }
}