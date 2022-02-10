using UnityEngine;
using Shared = AdapterPattern.Case2;

namespace AdapterPattern.Case2.Base1 {
  /// <summary>
  /// Using Serializable Interface
  /// </summary>
  public class ClientSurfaceAreaCalculator : Shared.ClientSurfaceAreaCalculator {
    [SerializeField]
    private ISurfaceAreaContainer _shape;

    public override void CalculateSurfaceArea() {
      print(_shape.Result.GetSurfaceArea());
    }
  }
}