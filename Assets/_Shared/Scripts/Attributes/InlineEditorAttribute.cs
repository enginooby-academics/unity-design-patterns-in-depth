using Sirenix.OdinInspector;

namespace Enginooby.Attribute {
#if ODIN_INSPECTOR
  /// <summary>
  ///   Unity 2021.2 already has built-in dropdown for serialized component.
  /// </summary>
  [IncludeMyAttributes]
  [Sirenix.OdinInspector.InlineEditor]
  public class InlineEditorAttribute : System.Attribute { }
  /// <inheritdoc />
  // public class InlineEditorAttribute : Sirenix.OdinInspector.InlineEditorAttribute {
  // }
#else
  [AttributeUsage(AttributeTargets.All, Inherited = false)]
  [Conditional("UNITY_EDITOR")]
  public class InlineEditorAttribute : PropertyAttribute {
    public InlineEditorAttribute() {
    }

    public InlineEditorAttribute(InlineEditorModes value) {
    }
  }

  public enum InlineEditorModes {
    FullEditor,
    GUIOnly
  }
#endif
}