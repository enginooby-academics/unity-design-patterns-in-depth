using UnityEngine;
using System.Collections.Generic;

public static class DebugUtils {
  //   public static void Log<T>(this T value) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
  // #if UNITY_EDITOR
  //     Debug.Log(value);
  // #endif
  //   }

  //   public static void Log(this string value) {
  // #if UNITY_EDITOR
  //     Debug.Log(value);
  // #endif
  //   }

  public static void Log(this System.Object value) {
#if UNITY_EDITOR
    // IMPL: get name of passing variable
    Debug.Log(value.ToString());
#endif
  }

  // REFACTOR
  public static void LogNames(this List<GameObject> gameObjects) {
    // TIP: Wrap all in-editor method implementations in #if UNITY_EDITOR
#if UNITY_EDITOR
    gameObjects.ForEach(gameObject => gameObject.name.Log());
#endif
  }

  public static void LogNames(this List<Transform> components) {
#if UNITY_EDITOR
    components.ForEach(component => component.name.Log());
#endif
  }

  public static void LogNames(this List<Component> components) {
#if UNITY_EDITOR
    components.ForEach(component => component.name.Log());
#endif
  }

  public static void LogNames(this Component[] components) {
#if UNITY_EDITOR
    foreach (var component in components) {
      component.name.Log();
    }
#endif
  }
}