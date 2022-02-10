#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

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
