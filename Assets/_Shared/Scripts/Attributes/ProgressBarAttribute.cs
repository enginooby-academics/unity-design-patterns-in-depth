using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class ProgressBarAttribute : PropertyAttribute {
    private float _min;
    private float _max;

    public ProgressBarAttribute(float min, float max) {
      _min = min;
      _max = max;
    }
  }
}
