using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class LabelTextAttribute : PropertyAttribute {
    private string _label;

    public LabelTextAttribute(string label) {
      _label = label;
    }
  }
}
