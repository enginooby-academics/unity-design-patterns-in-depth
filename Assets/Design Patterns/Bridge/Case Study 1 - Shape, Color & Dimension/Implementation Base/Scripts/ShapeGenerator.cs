using UnityEngine;
using Sirenix.OdinInspector;

namespace BridgePattern.Case1.Base {
  /// <summary>
  /// Client.
  /// </summary>
  public class ShapeGenerator : MonoBehaviour {
    [SerializeField]
    private ReferenceConcreteType<Shape> _shapeType;

    [SerializeField]
    private ReferenceConcreteType<IColor> _colorType;

    [SerializeField]
    private ReferenceConcreteType<IDimension> _dimensionType;

    [Button]
    public void GenerateShape() {
      Shape shape = _shapeType.CreateInstance(typeof(MeshFilter), typeof(MeshRenderer));
      shape.SetColor(_colorType.CreateInstance());
      shape.SetDimension(_dimensionType.CreateInstance());
      shape.Draw();
    }
  }
}
