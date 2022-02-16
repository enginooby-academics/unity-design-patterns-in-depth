using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginooby.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class ShowInInspectorAttribute : PropertyAttribute { }
}