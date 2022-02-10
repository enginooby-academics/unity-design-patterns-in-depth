using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
  [Conditional("UNITY_EDITOR")]
  public class HorizontalGroupAttribute : PropertyAttribute {
    public float Width;
    public float MarginLeft;
    public float MarginRight;
    public float PaddingLeft;
    public float PaddingRight;
    public float MinWidth;
    public float MaxWidth;
    public string Title;
    public float LabelWidth;

    public HorizontalGroupAttribute() {
    }

    public HorizontalGroupAttribute(string label, float value = 1f) {
    }
  }
}
