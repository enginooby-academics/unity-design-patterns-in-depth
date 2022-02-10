using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginoobz.Attribute {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class TitleAttribute : PropertyAttribute {
    private string _title;

    public TitleAttribute(string title, bool bold = false, TitleAlignments titleAlignment = TitleAlignments.Left) {
      _title = title;
    }
  }

  public enum TitleAlignments { Left }
}
