using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class ButtonAttribute : PropertyAttribute {
    public ButtonAttribute(ButtonSizes buttonSizes = ButtonSizes.Small) {
    }
  }

  public enum ButtonSizes {
    Small,
    Medium,
    Large
  }
}