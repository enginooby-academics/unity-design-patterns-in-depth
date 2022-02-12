using UnityEngine;

public static class InputUtils {
  public static float horizontalInput => Input.GetAxis("Horizontal");

  public static float verticalInput => Input.GetAxis("Vertical");

  public static bool IsHeld(this KeyCode keyCode) => Input.GetKey(keyCode);

  public static bool IsHeld(this MouseButton mouseButton) => Input.GetMouseButton((int) mouseButton);

  /// <summary>
  ///   If modifier key is not set, return true.
  /// </summary>
  public static bool IsHeld(this ModifierKey key) {
    if (key.HasFlag(ModifierKey.Lshift) && !Input.GetKey(KeyCode.LeftShift)) return false;
    if (key.HasFlag(ModifierKey.Lctrl) && !Input.GetKey(KeyCode.LeftControl)) return false;
    if (key.HasFlag(ModifierKey.Lalt) && !Input.GetKey(KeyCode.LeftAlt)) return false;
    if (key.HasFlag(ModifierKey.Rshift) && !Input.GetKey(KeyCode.RightShift)) return false;
    if (key.HasFlag(ModifierKey.Rctrl) && !Input.GetKey(KeyCode.RightControl)) return false;
    if (key.HasFlag(ModifierKey.Ralt) && !Input.GetKey(KeyCode.RightAlt)) return false;
    if (key.HasFlag(ModifierKey.Lmb) && !Input.GetKey(KeyCode.Mouse0)) return false;
    if (key.HasFlag(ModifierKey.Rmb) && !Input.GetKey(KeyCode.Mouse1)) return false;
    if (key.HasFlag(ModifierKey.Mmb) && !Input.GetKey(KeyCode.Mouse2)) return false;
    if (key.HasFlag(ModifierKey.Caps) && !Input.GetKey(KeyCode.CapsLock)) return false;
    return true;
  }

  public static bool IsUp(this KeyCode keyCode) => Input.GetKeyUp(keyCode);

  public static bool IsUp(this MouseButton mouseButton) => Input.GetMouseButtonUp((int) mouseButton);

  public static bool IsDown(this KeyCode keyCode) => Input.GetKeyDown(keyCode);

  public static bool IsDown(this MouseButton mouseButton) => Input.GetMouseButtonDown((int) mouseButton);

  public static bool MouseScrollUp() => Input.GetAxis("Mouse ScrollWheel") > 0f;

  public static bool MouseScrollDown() => Input.GetAxis("Mouse ScrollWheel") < 0f;

  public static float GetAxisVertical() => Input.GetAxis("Vertical");

  public static float GetAxisHorizontal() => Input.GetAxis("Horizontal");
}