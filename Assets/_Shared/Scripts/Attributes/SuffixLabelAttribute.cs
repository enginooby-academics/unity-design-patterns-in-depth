using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginooby.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class SuffixLabelAttribute : PropertyAttribute {
    private string _label;
    public bool Overlay;

    public SuffixLabelAttribute(string label) => _label = label;
  }
}