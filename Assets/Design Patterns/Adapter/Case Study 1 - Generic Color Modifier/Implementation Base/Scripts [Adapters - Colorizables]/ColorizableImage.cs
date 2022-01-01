using UnityEngine;
using UnityEngine.UI;

namespace AdapterPattern.Case1.Base1 {
  public class ColorizableImage : ColorizableObject<Image> {
    public ColorizableImage() { }

    public ColorizableImage(Image component) : base(component) { }

    public override Color Color { get => _object.color; set => _object.color = value; }
  }
}
