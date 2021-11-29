using UnityEngine;
using UnityEngine.UI;

namespace Adapter.Base {
  public class ColorizableText : IColorizable {
    private Text _text;

    public ColorizableText(Text text) {
      _text = text;
    }

    public Color Color { get => _text.color; set => _text.color = value; }
  }
}
