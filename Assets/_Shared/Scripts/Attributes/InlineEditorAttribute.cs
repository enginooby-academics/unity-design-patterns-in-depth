using Sirenix.OdinInspector;

namespace Enginoobz.Attribute {
#if ODIN_INSPECTOR
  [IncludeMyAttributes]
  [Sirenix.OdinInspector.InlineEditor]
  public class InlineEditorAttribute : System.Attribute {
  }
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