using System;

public enum MouseButton { Left, Right, Middle }

// * Use (EnumFlag)1 as None value, e.g., (ModifierKey)1
[Flags]
public enum ModifierKey {
  // * If add more ModifierKey, add its input valid IsHeld() in InputUtils.cs as well
  // L/R: Left/Right
  Lshift = 1 << 1,
  Rshift = 1 << 2,
  Lctrl = 1 << 3,
  Rctrl = 1 << 4,
  Caps = 1 << 5,
  Lalt = 1 << 6,
  Ralt = 1 << 7,
  // Mouse
  Lmb = 1 << 8,
  Rmb = 1 << 9,
  Mmb = 1 << 10,
}


public enum Axis { X, Y, Z }

[Flags]
public enum AxisFlag {
  X = 1 << 1, // 1
  Y = 1 << 2, // 2
  Z = 1 << 3, // 4
  ALL = X | Y | Z
}

[Flags]
public enum TriggerEventType { // CONSIDER: OnMouse events
  OnCollisionEnter = 1 << 1,
  OnCollisionExit = 1 << 2,
  OnTriggerEnter = 1 << 3,
  OnTriggerExit = 1 << 4,
  OnDestroy = 1 << 5,
  OnDisable = 1 << 6,
}

/* Command constants used for Quantum Console utility */
public static class CommandPrefix {
  public const string PostFx = "p.";
  public const string Environment = "e.";
  public const string Highlight = "h.";
  public const string MeshVfx = "mv.";
  public const string Selection = "s.";
}

public static class CommandTarget {
  public const string Selection = "selection";
  public const string Camera = "camera";
  public const string Tag = "tag";
}