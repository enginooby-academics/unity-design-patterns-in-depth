using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginooby.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
  [Conditional("UNITY_EDITOR")]
  public class ToggleGroupAttribute : PropertyAttribute {
    private string _condition;
    private string _groupTitle;

    public ToggleGroupAttribute(string condition, string groupTitle = "") {
      _condition = condition;
      _groupTitle = groupTitle;
    }
  }
}