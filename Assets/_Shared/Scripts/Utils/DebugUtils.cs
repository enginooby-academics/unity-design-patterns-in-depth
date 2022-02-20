using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

// ? Implement Logger wrapper to log in different ways + style
public static class DebugUtils {
  public static void Log<T>([CanBeNull] this T value) where T : IFormattable {
#if UNITY_EDITOR
    if (value is not null) Debug.Log(value.ToString());
#endif
  }

  public static void Log<T>([CanBeNull] this T? value)
    where T : unmanaged,
    IComparable,
    IComparable<T>,
    IConvertible,
    IEquatable<T>,
    IFormattable {
#if UNITY_EDITOR
    if (value is not null) Debug.Log(value.ToString());
#endif
  }


  public static void Log([CanBeNull] this string value) {
#if UNITY_EDITOR
    if (value is not null) Debug.Log(value);
#endif
  }

  public static void Log<T>(this IEnumerable<T> list) where T : IFormattable {
#if UNITY_EDITOR
    foreach (var item in list) item.Log();
#endif
  }

  public static void LogNames(this IEnumerable<Object> objects) {
#if UNITY_EDITOR
    foreach (var obj in objects) obj.name.Log();
#endif
  }
}