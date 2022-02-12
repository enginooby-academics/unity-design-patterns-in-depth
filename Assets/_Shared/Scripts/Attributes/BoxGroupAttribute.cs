using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class BoxGroupAttribute : PropertyAttribute {
    private string _label;

    public BoxGroupAttribute(string label) => _label = label;
  }
}