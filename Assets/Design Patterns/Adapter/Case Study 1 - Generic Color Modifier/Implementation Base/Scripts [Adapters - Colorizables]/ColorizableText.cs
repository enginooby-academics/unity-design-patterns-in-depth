using UnityEngine;
using UnityEngine.UI;

namespace AdapterPattern.Case1.Base1 {
  /// <summary>
  /// * [An 'Adapter' class]
  /// For Adaptee class: Text
  /// </summary>
  public class ColorizableText : ColorizableObject<Text> {
    public ColorizableText() { }

    public ColorizableText(Text component) : base(component) { }

    public override Color Color { get => _object.color; set => _object.color = value; }
  }
}
