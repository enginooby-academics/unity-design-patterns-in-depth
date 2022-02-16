using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginooby.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class LabelTextAttribute : PropertyAttribute {
    private string _label;

    public LabelTextAttribute(string label) => _label = label;
  }
}