using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginooby.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
  [Conditional("UNITY_EDITOR")]
  public class ShowIfGroupAttribute : PropertyAttribute {
    private string _condition;
    private object _optionalValue;

    public ShowIfGroupAttribute(string condition, object optionalValue) {
      _condition = condition;
      _optionalValue = optionalValue;
    }

    public ShowIfGroupAttribute(string condition) => _condition = condition;
  }
}