using UnityEngine;
using Shared = AdapterPattern.Case2;

namespace AdapterPattern.Case2.Base1 {
  public class ClientSurfaceAreaCalculator : Shared.ClientSurfaceAreaCalculator {
    [SerializeField]
    private ReferenceConcreteType<AreaToSurfaceAreaAdapter> _adapterType;

    public override void CalculateSurfaceArea() {
      double result = 0;

      if (_shape.TryGetComponent(typeof(ISurfaceArea), out var shape3dComponent)) {
        result = (shape3dComponent as ISurfaceArea).GetSurfaceArea();
      }

      if (_shape.TryGetComponent(typeof(IArea), out var shape2dComponent)) {
        // result = (shape2dComponent as IArea).GetArea();

        // ! With option 1
        // var adapted2dShape = new AreaToSurfaceAreaAdapterA(shape2dComponent as IArea);
        // result = adapted2dShape.GetSurfaceArea();

        // ! With option 1: reflection
        var adapted2dShape = _adapterType.CreateInstanceWithParams(shape2dComponent as IArea);
        result = adapted2dShape.GetSurfaceArea();

        // ! With option 2
        // var adapter = new Shape2DAdapterA();
        // result = adapter.GetSurfaceArea(shape2dComponent as IArea);

        // ! With option 3
        // result = Shape2DAdapterA.GetSurfaceArea(shape2dComponent as IArea);

        // ! With option 4
        // result = (shape2dComponent as IArea).GetSurfaceArea();
      }

      print(result);
    }
  }
}