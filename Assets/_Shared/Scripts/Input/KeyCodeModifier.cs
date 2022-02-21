using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

[Flags]
public enum KeyTriggerEvent {
  Up = 1 << 1,
  Down = 1 << 2,
  Hold = 1 << 3,
}

[Serializable]
[InlineProperty]
public class KeyCodeModifier {
  // https://answers.unity.com/questions/533541/unsupported-type-error-in-custom-editor-script.html
  // [HorizontalGroup(width: .3f)]
  [InlineButton(nameof(ToggleModifierAndTriggerAttributes), "...")]
  [InlineButton(nameof(SetKeyCodeNone), "Ø")]
  [InlineButton(nameof(IncreaseKeyCode), ">")]
  [InlineButton(nameof(DescreaseKeyCode), "<")]
  [HideLabel]
  public KeyCode keyCode;
  // [HorizontalGroup]

  [ShowIf(nameof(showModifierAndTriggerAttributes))] [HideLabel] [EnumToggleButtons]
  public ModifierKey modifierKey = (ModifierKey) 1;

  [ShowIf(nameof(showModifierAndTriggerAttributes))] [HideLabel] [EnumToggleButtons] [GUIColor(.6f, 1f, .6f, .8f)]
  public KeyTriggerEvent keyTriggerEvent = KeyTriggerEvent.Down;

  private bool showModifierAndTriggerAttributes;

  public KeyCodeModifier(
    KeyCode keyCode = KeyCode.None,
    ModifierKey modifierKey = (ModifierKey) 1,
    KeyTriggerEvent keyTriggerEvent = KeyTriggerEvent.Down) {
    this.keyCode = keyCode;
    this.modifierKey = modifierKey;
    this.keyTriggerEvent = keyTriggerEvent;
  }

  public bool IsTriggering =>
    keyTriggerEvent.HasFlag(KeyTriggerEvent.Down) && IsDown ||
    keyTriggerEvent.HasFlag(KeyTriggerEvent.Up) && IsUp ||
    keyTriggerEvent.HasFlag(KeyTriggerEvent.Hold) && IsHeld;

  public bool IsUp => modifierKey.IsHeld() && keyCode.IsUp();
  public bool IsDown => modifierKey.IsHeld() && keyCode.IsDown();
  public bool IsHeld => modifierKey.IsHeld() && keyCode.IsHeld();

  private void IncreaseKeyCode() {
    var keyCodeInt = (int) keyCode;
    keyCode = (KeyCode) (keyCodeInt + 1); // TODO: constrain max value
  }

  private void DescreaseKeyCode() {
    var keyCodeInt = (int) keyCode;
    if (keyCodeInt > 0) keyCode = (KeyCode) (keyCodeInt - 1);
  }

  private void SetKeyCodeNone() {
    keyCode = KeyCode.None;
  }

  private void ToggleModifierAndTriggerAttributes() {
    showModifierAndTriggerAttributes = !showModifierAndTriggerAttributes;
  }
}