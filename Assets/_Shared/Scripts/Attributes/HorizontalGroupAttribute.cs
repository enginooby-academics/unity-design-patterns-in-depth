using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
  [Conditional("UNITY_EDITOR")]
  public class HorizontalGroupAttribute : PropertyAttribute {
    public float LabelWidth;
    public float MarginLeft;
    public float MarginRight;
    public float MaxWidth;
    public float MinWidth;
    public float PaddingLeft;
    public float PaddingRight;
    public string Title;
    public float Width;

    public HorizontalGroupAttribute() {
    }

    public HorizontalGroupAttribute(string label, float value = 1f) {
    }
  }
}