using System;
using System.Diagnostics;
using UnityEngine;

namespace Enginooby.Attribute {
#if ASSET_NAUGHTY_ATTRIBUTES
  // public class TagAttribute : NaughtyAttributes.TagAttribute {
  //   // ! Not work
  // }
#else
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class TagAttribute : PropertyAttribute { }
#endif
}