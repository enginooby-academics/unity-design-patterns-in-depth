using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Reflection.Case1.Reflection {
  /// <summary>
  ///   Refine the ShapeManager using reusable ReferenceConcreteType.
  /// </summary>
  public class PrefabShapeManager : MonoBehaviour {
    // [SerializeField]
    // // [AssetList(Path = "/Design Patterns/")]
    // [AssetSelector(FlattenTreeView = true)]
    // private MonoBehaviourCube _monobehavourCube;

    [SerializeField] private IShapeContainer _shapePrefab;

    [Button]
    public void CreateShape() {
      var shape = Instantiate(_shapePrefab.Object) as IShape;
      print("Volume of the newly-created shape is: " + shape.GetVolume());
    }
  }

  [Serializable]
  public class IShapeContainer : IUnifiedContainer<IShape> { }
}