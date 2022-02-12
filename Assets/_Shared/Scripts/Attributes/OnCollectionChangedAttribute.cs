using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class OnCollectionChangedAttribute : PropertyAttribute {
    private string _handlerName;

    public OnCollectionChangedAttribute(string handlerName, bool value = false) => _handlerName = handlerName;
  }
}