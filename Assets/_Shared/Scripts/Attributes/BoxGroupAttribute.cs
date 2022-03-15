#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using System;
using System.Diagnostics;
using UnityEngine;
#endif

namespace Enginooby.Attribute {
#if ODIN_INSPECTOR
  /// <inheritdoc />
  public class BoxGroupAttribute : Sirenix.OdinInspector.BoxGroupAttribute { }
#else
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class BoxGroupAttribute : PropertyAttribute {
    private string _label;

    public BoxGroupAttribute(string label) => _label = label;
  }
#endif
}