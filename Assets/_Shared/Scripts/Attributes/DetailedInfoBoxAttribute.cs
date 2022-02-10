using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class DetailedInfoBoxAttribute : PropertyAttribute {
    public DetailedInfoBoxAttribute(string message, string details, InfoMessageType infoMessageType = InfoMessageType.Info, string visibleIf = null) {

    }
  }

}
