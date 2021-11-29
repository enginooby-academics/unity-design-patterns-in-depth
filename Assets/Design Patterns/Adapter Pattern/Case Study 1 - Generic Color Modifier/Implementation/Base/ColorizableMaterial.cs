using UnityEngine;

namespace Adapter.Base {
  public class ColorizableMaterial : IColorizable {
    private Material _material;

    public ColorizableMaterial(Material material) {
      _material = material;
    }

    public Color Color { get => _material.color; set => _material.color = value; }
  }
}
