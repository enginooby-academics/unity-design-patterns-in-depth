using Sirenix.OdinInspector;
using UnityEngine;

namespace GOConstruction.GUI {
  public class RefinedGUIShapeGenerator : MonoBehaviour {
    [SerializeField]
    private IShapeContainer _shapePrefab;

    [Button]
    public void CreateShape() {
      IShape shape = Instantiate(_shapePrefab.Object) as IShape;
      print("Volume of the generated shape is: " + shape.GetVolume());
    }
  }

  [System.Serializable]
  public class IShapeContainer : IUnifiedContainer<IShape> { }
}
