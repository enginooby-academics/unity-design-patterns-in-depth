using UnityEngine;

public static class InputUtils {
  public static bool IsHeld(this KeyCode keyCode) {
    return Input.GetKey(keyCode);
  }

  public static bool IsHeld(this MouseButton mouseButton) {
    return Input.GetMouseButton((int)mouseButton);
  }

  /// <summary>
  /// If modifier key is not set, return true.
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

  public static bool IsUp(this KeyCode keyCode) {
    return Input.GetKeyUp(keyCode);
  }

  public static bool IsUp(this MouseButton mouseButton) {
    return Input.GetMouseButtonUp(((int)mouseButton));
  }

  public static bool IsDown(this KeyCode keyCode) {
    return Input.GetKeyDown(keyCode);
  }

  public static bool IsDown(this MouseButton mouseButton) {
    return Input.GetMouseButtonDown(((int)mouseButton));
  }

  public static bool MouseScrollUp() {
    return Input.GetAxis("Mouse ScrollWheel") > 0f;
  }

  public static bool MouseScrollDown() {
    return Input.GetAxis("Mouse ScrollWheel") < 0f;
  }

  public static float GetAxisVertical() {
    return Input.GetAxis("Vertical");
  }

  public static float GetAxisHorizontal() {
    return Input.GetAxis("Horizontal");
  }

  public static float horizontalInput {
    get { return Input.GetAxis("Horizontal"); }
  }

  public static float verticalInput {
    get { return Input.GetAxis("Vertical"); }
  }
}