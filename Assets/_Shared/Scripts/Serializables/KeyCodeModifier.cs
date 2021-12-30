using UnityEngine;
using Sirenix.OdinInspector;
using System;

[Flags] public enum KeyTriggerEvent { Up = 1 << 1, Down = 1 << 2, Hold = 1 << 3 }

[Serializable, InlineProperty]
public class KeyCodeModifier {
  // https://answers.unity.com/questions/533541/unsupported-type-error-in-custom-editor-script.html
  // [HorizontalGroup(width: .3f)]
  [InlineButton(nameof(ToggleModifierAndTriggerAttributes), label: "...")]
  [InlineButton(nameof(SetKeyCodeNone), label: "Ø")]
  [InlineButton(nameof(IncreaseKeyCode), ">")]
  [InlineButton(nameof(DescreaseKeyCode), "<")]
  [HideLabel] public KeyCode keyCode;
  private void IncreaseKeyCode() {
    int keyCodeInt = (int)keyCode;
    keyCode = (KeyCode)(keyCodeInt + 1); // TODO: constrain max value
  }
  private void DescreaseKeyCode() {
    int keyCodeInt = (int)keyCode;
    if (keyCodeInt > 0) keyCode = (KeyCode)(keyCodeInt - 1);
  }
  private void SetKeyCodeNone() {
    keyCode = KeyCode.None;
  }
  // [HorizontalGroup]

  [ShowIf(nameof(showModifierAndTriggerAttributes))]
  [HideLabel, EnumToggleButtons] public ModifierKey modifierKey = (ModifierKey)1;
  [ShowIf(nameof(showModifierAndTriggerAttributes))]
  [HideLabel, EnumToggleButtons, GUIColor(.6f, 1f, .6f, .8f)] public KeyTriggerEvent keyTriggerEvent = KeyTriggerEvent.Down;
  private bool showModifierAndTriggerAttributes = false;
  private void ToggleModifierAndTriggerAttributes() {
    showModifierAndTriggerAttributes = !showModifierAndTriggerAttributes;
  }
  public KeyCodeModifier(KeyCode keyCode = KeyCode.None, ModifierKey modifierKey = (ModifierKey)1, KeyTriggerEvent keyTriggerEvent = KeyTriggerEvent.Down) {
    this.keyCode = keyCode;
    this.modifierKey = modifierKey;
    this.keyTriggerEvent = keyTriggerEvent;
  }

  public bool IsTriggering {
    get =>
    (keyTriggerEvent.HasFlag(KeyTriggerEvent.Down) && IsDown) ||
    (keyTriggerEvent.HasFlag(KeyTriggerEvent.Up) && IsUp) ||
    (keyTriggerEvent.HasFlag(KeyTriggerEvent.Hold) && IsHeld);
  }

  public bool IsUp { get => modifierKey.IsHeld() && keyCode.IsUp(); }
  public bool IsDown { get => modifierKey.IsHeld() && keyCode.IsDown(); }
  public bool IsHeld { get => modifierKey.IsHeld() && keyCode.IsHeld(); }
}