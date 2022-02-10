using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class MinMaxSliderAttribute : PropertyAttribute {
    private float _min;
    private float _max;
    private bool _showFields;

    public MinMaxSliderAttribute(float min, float max, bool showFields = false) {
      _min = min;
      _max = max;
      _showFields = showFields;
    }

    public MinMaxSliderAttribute(string minMaxGetter, bool showFields = false) {
      _showFields = showFields;
    }

    public MinMaxSliderAttribute(string minGetter, string maxGetter, bool showFields = false) {
      _showFields = showFields;
    }
  }
}
