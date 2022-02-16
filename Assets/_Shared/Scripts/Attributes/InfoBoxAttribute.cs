using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginooby.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class InfoBoxAttribute : PropertyAttribute {
    private string _text;

    public InfoBoxAttribute(string text, InfoMessageType infoMessageType = InfoMessageType.Info) => _text = text;
  }

  public enum InfoMessageType {
    Warning,
    Info,
  }
}