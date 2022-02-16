using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginooby.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class OnValueChangedAttribute : PropertyAttribute {
    private string _handlerName;

    public OnValueChangedAttribute(string handlerName, bool value = false) => _handlerName = handlerName;
  }
}