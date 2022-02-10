using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class InlineEditorAttribute : PropertyAttribute {
    public InlineEditorAttribute() {
    }

    public InlineEditorAttribute(InlineEditorModes value) {
    }
  }

  public enum InlineEditorModes { FullEditor, GUIOnly }
}
