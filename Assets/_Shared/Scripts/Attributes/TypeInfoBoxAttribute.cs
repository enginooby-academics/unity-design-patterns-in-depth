using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class TypeInfoBoxAttribute : PropertyAttribute {
    private string _text;

    public TypeInfoBoxAttribute(string text, InfoMessageType infoMessageType = InfoMessageType.Info) => _text = text;
  }
}