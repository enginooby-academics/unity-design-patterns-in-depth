using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
  [Conditional("UNITY_EDITOR")]
  public class EnableIfAttribute : PropertyAttribute {
    private string _condition;
    private object _optionalValue;

    public EnableIfAttribute(string condition, object optionalValue) {
      _condition = condition;
      _optionalValue = optionalValue;
    }

    public EnableIfAttribute(string condition) => _condition = condition;
  }
}