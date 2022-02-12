using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class MinValueAttribute : PropertyAttribute {
    private float _value;

    public MinValueAttribute(float value) => _value = value;
  }
}