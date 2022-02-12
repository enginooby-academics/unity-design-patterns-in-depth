using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class DisplayAsStringAttribute : PropertyAttribute {
    public DisplayAsStringAttribute() {
    }

    public DisplayAsStringAttribute(bool value) {
    }
  }
}