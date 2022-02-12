using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class PropertyOrderAttribute : PropertyAttribute {
    private int _order;

    public PropertyOrderAttribute(int order) => _order = order;
  }
}