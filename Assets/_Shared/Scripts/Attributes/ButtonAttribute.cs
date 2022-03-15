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
  public class ButtonAttribute : Sirenix.OdinInspector.ButtonAttribute { }
#else
  // Fallback attribute

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
#endif
}