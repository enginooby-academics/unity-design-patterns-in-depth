using Sirenix.OdinInspector;
using UnityEngine;

namespace Reflection.Case1.Reflection {
  /// <summary>
  /// Refine the ShapeManager using reusable ReferenceConcreteType.
  /// </summary>
  public class RefinedShapeManager : MonoBehaviour {
    [SerializeField]
    private ReferenceConcreteType<IShape> _currentshapeType;

    [Button]
    public void CreateShape() {
      IShape shape = _currentshapeType.CreateInstance(typeof(MeshFilter), typeof(MeshRenderer));
      print("Volume of the newly-created shape is: " + shape.GetVolume());
    }
  }
}
