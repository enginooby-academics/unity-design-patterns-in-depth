using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class GUIColorAttribute : PropertyAttribute {
    private string _colorExpression;

    public GUIColorAttribute(string colorExpression) => _colorExpression = colorExpression;

    public GUIColorAttribute(float r, float g, float b, float a = 1f) {
    }
  }
}