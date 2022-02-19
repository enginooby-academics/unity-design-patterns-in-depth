using System;
using JetBrains.Annotations;
using UnityEngine.UI;

public static class UIUtils {
  // TIP: Use CanBeNull attribute so that we don't need to bother about NullException in usage
  // E.g., usage with CanBeNull: _healthLabel.SetText(_heath);
  // Without CanBeNull: _heathLabel?.SetText(_health);  Besides, we may have to deal with:
  // "'?.' on a type deriving from 'UnityEngine.Object' bypasses the lifetime check on the underlying Unity engine object"
  public static void SetText([CanBeNull] this Text label, string text) {
    if (label) label.text = text;
  }

  // TIP: Create another method along w/ string-related method using IFormattable to support all cases
  // Without this, we have to explicitly invoke ToString()
  public static void SetText<T>([CanBeNull] this Text label, T text) where T : IFormattable {
    label.SetText(text.ToString());
  }
}