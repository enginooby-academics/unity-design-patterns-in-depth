using UnityEngine;
using Sirenix.OdinInspector;

namespace BridgePattern.Case1.Base {
  /// <summary>
  /// Client.
  /// </summary>
  public class ShapeGenerator : MonoBehaviour {
    [SerializeField]
    private ReferenceType<Shape> _shapeType;

    [SerializeField]
    private ReferenceType<IColor> _colorType;

    [SerializeField]
    private ReferenceType<IDimension> _dimensionType;

    [Button]
    public void GenerateShape() {
      Shape shape = _shapeType.CreateInstance();
      shape.SetColor(_colorType.CreateInstance());
      shape.SetDimension(_dimensionType.CreateInstance());
      shape.Draw();
    }
  }
}
