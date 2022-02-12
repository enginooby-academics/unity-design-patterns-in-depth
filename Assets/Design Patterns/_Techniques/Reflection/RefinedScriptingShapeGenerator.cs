using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

namespace GOConstruction.Scripting {
  public class RefinedScriptingShapeGenerator : MonoBehaviour {
    [SerializeField] private ReferenceConcreteType<IShape> _shapeType;

    [Button]
    public void CreateShape() {
      var shape = _shapeType.CreateInstance();
      print("Volume of the generated shape is: " + shape.GetVolume());
    }
  }
}