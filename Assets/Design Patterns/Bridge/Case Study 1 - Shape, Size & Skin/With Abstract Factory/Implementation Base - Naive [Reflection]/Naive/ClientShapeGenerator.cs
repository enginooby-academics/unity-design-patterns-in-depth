#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;
using System;

namespace BridgePattern.Case1.Base.WithAbstractFactory.Naive {
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
      ConstraintShapeType();
      Shape shape = _shapeType.CreateInstance(typeof(MeshFilter), typeof(MeshRenderer));
      shape.SetSize(_sizeType.CreateInstance());
      shape.SetSkin(_skinType.CreateInstance());
      shape.Display();
    }

    private void ConstraintShapeType() {
      if (_shapeType.Is<Cube>() && _sizeType.Is<Big>()) {
        throw new InvalidOperationException("Cannot create big cube");
      }

      if (_shapeType.Is<Sphere>() && _skinType.Is<Dark>()) {
        throw new InvalidOperationException("Cannot create dark sphere");
      }
    }
  }
}
