using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class LabelWidthAttribute : PropertyAttribute {
    private float _width;

    public LabelWidthAttribute(float width) => _width = width;
  }
}