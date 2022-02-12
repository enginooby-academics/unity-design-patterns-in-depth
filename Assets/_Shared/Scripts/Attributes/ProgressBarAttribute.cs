using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class ProgressBarAttribute : PropertyAttribute {
    private float _max;
    private float _min;

    public ProgressBarAttribute(float min, float max) {
      _min = min;
      _max = max;
    }
  }
}