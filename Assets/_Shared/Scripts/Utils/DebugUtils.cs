using UnityEngine;
using System;
using System.Collections.Generic;

public static class DebugUtils {
  public static void Log<T>(this T value) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
#if UNITY_EDITOR
    Debug.Log(value);
#endif
  }

  public static void Log(this string value) {
#if UNITY_EDITOR
    Debug.Log(value);
#endif
  }

  public static void LogNames(this List<Transform> list) {
#if UNITY_EDITOR
    list.ForEach(transform => transform.name.Log());
#endif
  }
}