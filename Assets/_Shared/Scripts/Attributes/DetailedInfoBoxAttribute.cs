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
  public class DetailedInfoBoxAttribute : Sirenix.OdinInspector.DetailedInfoBoxAttribute {
    public DetailedInfoBoxAttribute(
      string message,
      string details,
      Sirenix.OdinInspector.InfoMessageType infoMessageType = Sirenix.OdinInspector.InfoMessageType.Info,
      string visibleIf = null)
      : base(message, details, infoMessageType, visibleIf) { }
  }
#else
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class DetailedInfoBoxAttribute : PropertyAttribute {
    public DetailedInfoBoxAttribute(string message, string details,
      InfoMessageType infoMessageType = InfoMessageType.Info, string visibleIf = null) {
    }
  }
#endif
}