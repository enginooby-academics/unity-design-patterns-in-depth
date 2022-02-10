#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;

namespace BridgePattern.Case1.Naive2 {
  public class ClientShapeGenerator : MonoBehaviour {
    [SerializeField]
    private ReferenceConcreteType<Shape> _shapeType;

    [Button]
    public void GenerateShape() => _shapeType.CreateInstance();
  }
}
