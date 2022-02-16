using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginooby.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class FoldoutGroupAttribute : PropertyAttribute {
    private string _label;

    public FoldoutGroupAttribute(string label) => _label = label;
  }
}