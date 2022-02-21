using UnityEngine;

namespace Enginooby.Utils {
  /// <summary>
  ///   Provide type-safe access to axes in the Input Manager.
  /// </summary>
  public static class InputAxis {
    // ? Performance cost with string casting
    public static float Horizontal => Input.GetAxis(global::InputAxis.Horizontal.ToString());

    public static float Vertical => Input.GetAxis(global::InputAxis.Vertical.ToString());
  }
}