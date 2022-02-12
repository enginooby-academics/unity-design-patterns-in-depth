using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

namespace Reflection.Case1.Reflection {
  /// <summary>
  ///   Refine the ShapeManager using reusable ReferenceConcreteType.
  /// </summary>
  public class RefinedShapeManager : MonoBehaviour {
    [SerializeField] private ReferenceConcreteType<IShape> _currentshapeType;

    [Button]
    public void CreateShape() {
      var shape = _currentshapeType.CreateInstance(typeof(MeshFilter), typeof(MeshRenderer));
      print("Volume of the newly-created shape is: " + shape.GetVolume());
    }
  }
}