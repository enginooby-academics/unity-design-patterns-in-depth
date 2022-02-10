#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using UnityEngine;

namespace Reflection.Case1.Reflection {
  /// <summary>
  /// Refine the ShapeManager using reusable ReferenceConcreteType.
  /// </summary>
  public class PrefabShapeManager : MonoBehaviour {
    // [SerializeField]
    // // [AssetList(Path = "/Design Patterns/")]
    // [AssetSelector(FlattenTreeView = true)]
    // private MonoBehaviourCube _monobehavourCube;

    [SerializeField]
    private IShapeContainer _shapePrefab;

    [Button]
    public void CreateShape() {
      IShape shape = Instantiate(_shapePrefab.Object) as IShape;
      print("Volume of the newly-created shape is: " + shape.GetVolume());
    }
  }

  [System.Serializable]
  public class IShapeContainer : IUnifiedContainer<IShape> { }
}
