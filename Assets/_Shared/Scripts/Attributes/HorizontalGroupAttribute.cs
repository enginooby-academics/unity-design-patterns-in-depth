#if ODIN_INSPECTOR
#endif

namespace Enginooby.Attribute {
#if ODIN_INSPECTOR
  // [IncludeMyAttributes]
  // [HorizontalGroup]
  // public class HorizontalGroupAttribute : System.Attribute {
  // }
  /// <inheritdoc />
  public class HorizontalGroupAttribute : Sirenix.OdinInspector.HorizontalGroupAttribute { }
#else
  [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
  [Conditional("UNITY_EDITOR")]
  public class HorizontalGroupAttribute : PropertyAttribute {
    public float LabelWidth;
    public float MarginLeft;
    public float MarginRight;
    public float MaxWidth;
    public float MinWidth;
    public float PaddingLeft;
    public float PaddingRight;
    public string Title;
    public float Width;

    public HorizontalGroupAttribute() {
    }

    public HorizontalGroupAttribute(string label, float value = 1f) {
    }
  }
#endif
}