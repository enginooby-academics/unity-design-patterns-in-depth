using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class DebugUtils {
  // TIP: IFormattable for all types with ToString()
  public static void Log<T>(this IList<T> list) where T : IFormattable {
#if UNITY_EDITOR
    foreach (var item in list) Debug.Log(item.ToString());
#endif
  }

  public static void Log<T>(this T value) where T : IFormattable {
#if UNITY_EDITOR
    Debug.Log(value.ToString());
#endif
  }

  public static void Log(this string value) {
#if UNITY_EDITOR
    Debug.Log(value);
#endif
  }

  // REFACTOR
  public static void LogNames(this IList<Object> objects) {
#if UNITY_EDITOR
    foreach (var obj in objects) obj.name.Log();
#endif
  }

  public static void LogNames(this Object[] components) {
#if UNITY_EDITOR
    foreach (var component in components) component.name.Log();
#endif
  }
}