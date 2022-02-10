using UnityEngine;

namespace AdapterPattern.Case1.Base1 {
  /// <summary>
  /// * [An 'Adapter' class]
  /// For Adaptee class: Material
  /// </summary>
  public class ColorizableMaterial : ColorizableObject<Material, MeshRenderer> {
    public ColorizableMaterial() { }

    public ColorizableMaterial(MeshRenderer component) : base(component) {
      _object = _component.material;
    }

    public override Color Color { get => _object.color; set => _object.color = value; }
  }
}
