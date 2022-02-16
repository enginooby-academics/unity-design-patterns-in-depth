using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginooby.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
  [Conditional("UNITY_EDITOR")]
  public class InlineButtonAttribute : PropertyAttribute {
    private string _action;
    private string _label;

    public InlineButtonAttribute(string action, string label = null) {
      _action = action;
      _label = label;
    }
  }
}