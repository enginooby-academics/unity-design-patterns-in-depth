using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class TabGroupAttribute : PropertyAttribute {
    private string _label;

    public TabGroupAttribute(string label) => _label = label;

    public TabGroupAttribute(string label, string tab) => _label = label;
  }
}