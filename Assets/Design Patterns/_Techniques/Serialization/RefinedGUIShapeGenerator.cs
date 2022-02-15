using System;
using Enginoobz.Attribute;
using UnityEngine;

namespace GOConstruction.GUI {
  public class RefinedGUIShapeGenerator : MonoBehaviour {
    [SerializeField] private IShapeContainer _shapePrefab;

    [Button]
    public void CreateShape() {
      var shape = Instantiate(_shapePrefab.Object) as IShape;
      print("Volume of the generated shape is: " + shape.GetVolume());
    }
  }

  // !
  [Serializable]
  public class IShapeContainer : IUnifiedContainer<IShape> { }
}