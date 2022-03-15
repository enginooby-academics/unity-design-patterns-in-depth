#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using UnityEngine.UIElements;

namespace Enginooby.Utils {
  public static class UIElementsUtils {
    public static VisualElement AddButton(this VisualElement container, string label, StyleSheet styleSheet = null) {
      var button = new Button {text = label};
      return container.AddVisualElement(button, styleSheet);
    }

#if UNITY_EDITOR
    public static VisualElement AddColorField(this VisualElement container, StyleSheet styleSheet = null) {
      var colorField = new ColorField();
      return container.AddVisualElement(colorField, styleSheet);
    }
#endif

    public static VisualElement AddLabel(this VisualElement container, string text, StyleSheet styleSheet = null) {
      var label = new Label(text);
      return container.AddVisualElement(label, styleSheet);
    }

    /// <summary>
    /// Apply stylesheet to child element and add it to the container element.
    /// <returns>Child element.</returns>
    /// </summary>
    public static VisualElement AddVisualElement(
      this VisualElement container,
      VisualElement child,
      StyleSheet styleSheet = null) {
      if (styleSheet != null) child.styleSheets.Add(styleSheet);
      container.Add(child);
      return child;
    }
  }
}