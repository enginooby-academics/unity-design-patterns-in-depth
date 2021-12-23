using Sirenix.OdinInspector;
using UnityEngine;

namespace BridgePattern.Case1.Naive {
  public class ClientShapeGenerator : MonoBehaviour {
    [SerializeField]
    private ReferenceConcreteType<IShape> _shapeType;

    [Button]
    public void GenerateShape() {
      IShape shape = _shapeType.CreateInstance();
      shape.Display();
    }
  }
}
