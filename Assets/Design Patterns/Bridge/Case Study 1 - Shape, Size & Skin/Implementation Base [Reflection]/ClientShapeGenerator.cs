#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;

namespace BridgePattern.Case1.Base {
  /// <summary>
  /// * [The 'Client' class]
  /// </summary>
  public class ClientShapeGenerator : MonoBehaviour {
    [SerializeField]
    private ReferenceConcreteType<Shape> _shapeType;

    [SerializeField]
    private ReferenceConcreteType<ISize> _sizeType;

    [SerializeField]
    private ReferenceConcreteType<ISkin> _skinType;

    [Button]
    public void GenerateShape() {
      Shape shape = _shapeType.CreateInstance(typeof(MeshFilter), typeof(MeshRenderer));
      shape.SetSize(_sizeType.CreateInstance());
      shape.SetSkin(_skinType.CreateInstance());
      shape.Display();
    }
  }
}
