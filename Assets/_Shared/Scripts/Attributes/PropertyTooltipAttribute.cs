using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class PropertyTooltipAttribute : PropertyAttribute {
    private string _label;

    public PropertyTooltipAttribute(string label) {
      _label = label;
    }
  }
}
