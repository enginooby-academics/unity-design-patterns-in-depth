using Sirenix.OdinInspector;
using UnityEngine;

namespace GOConstruction.Scripting {
  public class RefinedScriptingShapeGenerator : MonoBehaviour {
    [SerializeField]
    private ReferenceConcreteType<IShape> _shapeType;

    [Button]
    public void CreateShape() {
      IShape shape = _shapeType.CreateInstance();
      print("Volume of the generated shape is: " + shape.GetVolume());
    }
  }
}
