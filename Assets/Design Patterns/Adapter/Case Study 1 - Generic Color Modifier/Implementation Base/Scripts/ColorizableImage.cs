using UnityEngine;
using UnityEngine.UI;

namespace Adapter.Base {
public class ColorizableImage : IColorizable {
  private Image _image;

  public ColorizableImage(Image image) {
    _image = image;
  }

  public Color Color { get => _image.color; set => _image.color = value; }
}
}
